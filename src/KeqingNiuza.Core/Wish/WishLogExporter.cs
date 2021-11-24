using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.Wish
{
    public class WishLogExporter
    {
        private readonly string baseRequestUrl = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog";

        private readonly string authString;

        private readonly HttpClient HttpClient;

        public event EventHandler<string> ProgressChanged;

        private static Random random = new Random();

        public WishLogExporter(string url)
        {
            if (url.StartsWith("https://") && url.EndsWith("#/log"))
            {
                authString = url.Substring(url.IndexOf('?')).Replace("#/log", "");
                HttpClient = new HttpClient();
                if (url.Contains("webstatic-sea"))
                {
                    baseRequestUrl = @"https://hk4e-api-os.mihoyo.com/event/gacha_info/api/getGachaLog";
                }
            }
            else
            {
                throw new ArgumentException("Url不符合要求");
            }
        }

        /// <summary>
        /// 获取祈愿记录数据
        /// </summary>
        /// <param name="size">每次Api请求获取几条数据，默认6条，最多20条</param>
        /// <param name="lastId">本地最新的id，获取的祈愿id小于最新id即停止</param>
        /// <returns></returns>
        public async Task<List<WishData>> GetAllLog(int size = 6, long lastId = 0)
        {
            List<WishData> wishDatas = new List<WishData>();
            await Task.Run(() =>
            {
                QueryParam param = new QueryParam() { WishType = WishType.Novice, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param, lastId));
                param = new QueryParam() { WishType = WishType.Permanent, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param, lastId));
                param = new QueryParam() { WishType = WishType.CharacterEvent, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param, lastId));
                param = new QueryParam() { WishType = WishType.WeaponEvent, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param, lastId));
            });
            return wishDatas;
        }


        public List<WishData> GetWishLogList(WishType type)
        {
            QueryParam param = new QueryParam() { WishType = type, Page = 1, Size = 6, EndId = 0 };
            return GetWishLogList(param);
        }


        /// <summary>
        /// 获取Url所属的Uid
        /// </summary>
        /// <exception cref="Exception">没有祈愿记录</exception>
        /// <returns></returns>
        public async Task<int> GetUidByUrl()
        {
            var list = new List<WishData>();
            ProgressChanged?.Invoke(this, "正在获取祈愿记录网址对应的 Uid");
            await Task.Run(() =>
             {
                 QueryParam param = new QueryParam() { WishType = WishType.CharacterEvent, Page = 1, Size = 6, EndId = 0 };
                 list.AddRange(GetWishLog(param));
                 param = new QueryParam() { WishType = WishType.Permanent, Page = 1, Size = 6, EndId = 0 };
                 list.AddRange(GetWishLog(param));
                 param = new QueryParam() { WishType = WishType.WeaponEvent, Page = 1, Size = 6, EndId = 0 };
                 list.AddRange(GetWishLog(param));
                 param = new QueryParam() { WishType = WishType.Novice, Page = 1, Size = 6, EndId = 0 };
                 list.AddRange(GetWishLog(param));
             });
            if (list.Any())
            {
                return list.First().Uid;
            }
            else
            {
                throw new ArgumentException("此Uid没有祈愿记录");
            }
        }


        /// <summary>
        /// 获取指定类型的所有祈愿记录，或截止到指定id
        /// </summary>
        /// <param name="param">祈愿类型</param>
        /// <param name="lastId">截止到的祈愿id</param>
        /// <returns></returns>
        private List<WishData> GetWishLogList(QueryParam param, long lastId = 0)
        {
            List<WishData> list = new List<WishData>();
            string url, str;
            ResponseData result;
            do
            {
                OnProgressChanged(param);
                Task.Delay(random.Next(200, 300)).Wait();
                url = $@"{baseRequestUrl}{authString}&{param}";
                str = HttpClient.GetStringAsync(url).Result;
                result = JsonSerializer.Deserialize<ResponseData>(str);
                if (result.Retcode != 0)
                {
                    throw new ArgumentException(result.Message);
                }
                if (result.Data.List.Count != 0)
                {
                    list.AddRange(result.Data.List);
                    param.Page++;
                    param.EndId = result.Data.List.Last().Id;
                    if (param.EndId <= lastId)
                    {
                        break;
                    }
                }

            } while (result.Data.List.Count == param.Size);
            return list;
        }

        /// <summary>
        /// 获取一页祈愿数据
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        private List<WishData> GetWishLog(QueryParam param)
        {
            Task.Delay(random.Next(200, 300)).Wait();
            List<WishData> list = new List<WishData>();
            var url = $@"{baseRequestUrl}{authString}&{param}";
            var str = HttpClient.GetStringAsync(url).Result;
            var result = JsonSerializer.Deserialize<ResponseData>(str);
            if (result.Retcode != 0)
            {
                throw new ArgumentException(result.Message);
            }
            if (result.Data.List.Count != 0)
            {
                list.AddRange(result.Data.List);
            }
            return list;
        }


        private void OnProgressChanged(QueryParam param)
        {
            string type = null;
            switch (param.WishType)
            {
                case WishType.Novice:
                    type = "新手";
                    break;
                case WishType.Permanent:
                    type = "常驻";
                    break;
                case WishType.CharacterEvent:
                    type = "角色";
                    break;
                case WishType.WeaponEvent:
                    type = "武器";
                    break;
            }
            ProgressChanged?.Invoke(this, $"正在获取 {type}祈愿 第 {param.Page} 页");
        }
    }
}
