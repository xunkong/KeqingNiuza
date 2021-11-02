using DGP.Genshin.Common.Request;
using DGP.Genshin.Common.Request.QueryString;
using DGP.Genshin.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    /// <summary>
    /// 联机抽卡记录
    /// </summary>
    public class GachaLogWorker
    {
        public GachaDataCollection WorkingGachaData { get; set; }

        private string workingUid;

        #region Initialization
        private readonly int batchSize;
        private readonly string gachaLogUrl;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gachaLogUrl">url</param>
        /// <param name="gachaData">需要操作的祈愿数据</param>
        /// <param name="batchSize">每次请求获取的批大小</param>
        public GachaLogWorker(string gachaLogUrl, GachaDataCollection gachaData, int batchSize = 20)
        {
            this.gachaLogUrl = gachaLogUrl;
            WorkingGachaData = gachaData;
            this.batchSize = batchSize;
        }
        #endregion

        private Config gachaConfig;
        private (int min, int max) delay = (1000, 2000);

        public async Task<Config> GetCurrentGachaConfigAsync()
        {
            if (gachaConfig == null)
            {
                gachaConfig = await GetGachaConfigAsync();
            }

            return gachaConfig;
        }

        /// <summary>
        /// 设置祈愿接口获取延迟是否启用
        /// </summary>
        public bool IsFetchDelayEnabled { get; set; } = true;

        /// <summary>
        /// 随机延迟的范围
        /// </summary>
        public (int min, int max) Delay
        {
            get => delay; set
            {
                if (value.min > value.max)
                {
                    throw new InvalidOperationException("最小值不能大于最大值");
                }
                delay = value;
            }
        }

        /// <summary>
        /// 获取祈愿池信息
        /// </summary>
        /// <returns>网络问题导致的可能会返回null</returns>
        private async Task<Config> GetGachaConfigAsync()
        {
            Requester requester = new Requester(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 }
            });
            Response<Config> resp = await requester.GetAsync<Config>(gachaLogUrl?.Replace("getGachaLog?", "getConfigList?"));
            return resp?.Data;
        }
        /// <summary>
        /// 随机延时用
        /// </summary>
        private readonly Random random = new Random();

        public event Action<FetchProgress> OnFetchProgressed;

        /// <summary>
        /// 获取单个奖池的祈愿记录增量信息
        /// 并自动合并数据
        /// </summary>
        /// <param name="type">卡池类型</param>
        public async Task FetchGachaLogIncrementAsync(ConfigType type)
        {
            List<GachaLogItem> increment = new List<GachaLogItem>();
            int currentPage = 0;
            long endId = 0;
            do
            {
                (bool Succeed, GachaLog log) = await TryGetBatchAsync(type, endId, ++currentPage);
                if (Succeed)
                {
                    if (log.List != null)
                    {
                        foreach (GachaLogItem item in log.List)
                        {
                            workingUid = item.Uid;
                            //this one is increment
                            if (item.TimeId > WorkingGachaData.GetNewestTimeId(type, item.Uid))
                            {
                                increment.Add(item);
                            }
                            else//already done the new item
                            {
                                MergeIncrement(type, increment);
                                return;
                            }
                        }
                        //last page
                        if (log.List.Count < batchSize)
                        {
                            break;
                        }
                        endId = log.List.Last().TimeId;
                    }
                }
                else
                {
                    //url not valid
                    throw new InvalidOperationException("提供的Url无效");
                }
                if (IsFetchDelayEnabled)
                {
                    Task.Delay(GetRandomDelay()).Wait();
                }
            } while (true);
            //first time fecth could go here
            MergeIncrement(type, increment);
        }

        private int GetRandomDelay()
        {
            return Delay.min + random.Next(Delay.max - Delay.min, Delay.max);
        }

        /// <summary>
        /// 合并增量
        /// </summary>
        /// <param name="type">卡池类型</param>
        /// <param name="increment">增量</param>
        private void MergeIncrement(ConfigType type, List<GachaLogItem> increment)
        {
            if (workingUid is null)
            {
                throw new InvalidOperationException($"{nameof(workingUid)} 不应为 null");
            }
            if (!WorkingGachaData.ContainsKey(workingUid))
            {
                WorkingGachaData.Add(workingUid, new GachaData());
            }
            //简单的将老数据插入到增量后侧，最后重置数据
            GachaData data = WorkingGachaData[workingUid];
            string key = type.Key;
            if (key != null)
            {
                if (data.ContainsKey(key))
                {
                    List<GachaLogItem> local = data[key];
                    if (local != null)
                    {
                        increment.AddRange(local);
                    }
                }
                data[key] = increment;
            }
        }

        /// <summary>
        /// 尝试获得20个奖池物品
        /// </summary>
        /// <param name="result">空列表或包含数据的列表</param>
        /// <param name="type"></param>
        /// <param name="endId"></param>
        /// <returns></returns>
        private async Task<(bool Succeed, GachaLog GachaLog)> TryGetBatchAsync(ConfigType type, long endId, int currentPage)
        {
            OnFetchProgressed?.Invoke(new FetchProgress() { Type = type.Name, Page = currentPage });
            //modify the url
            string[] splitedUrl = gachaLogUrl?.Split('?');
            string baseUrl = splitedUrl?[0];

            //parse querystrings
            QueryString query = QueryString.Parse(splitedUrl?[1]);
            query.Set("gacha_type", type.Key);
            //20 is the max size the api can return
            query.Set("size", batchSize.ToString());
            query.Set("lang", "zh-cn");
            query.Set("end_id", endId.ToString());
            string finalUrl = $"{baseUrl}?{query}";

            Requester requester = new Requester(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 }
            });
            Response<GachaLog> resp = await requester.GetAsync<GachaLog>(finalUrl);

            return resp?.ReturnCode == 0 ? (true, resp.Data ?? new GachaLog()) : (false, new GachaLog());
        }
    }
}
