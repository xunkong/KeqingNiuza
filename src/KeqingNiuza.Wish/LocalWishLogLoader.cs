using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeqingNiuza.Wish
{
    public class LocalWishLogLoader
    {
        /// <summary>
        /// 从本地文件中加载祈愿记录，自动完成总次数和保底内的计算，以id升序排序
        /// </summary>
        /// <param name="file">文件地址</param>
        /// <returns></returns>
        public static List<WishData> Load(string file)
        {
            var WishEventList = Const.WishEventList;
            var CharacterInfoList = Const.CharacterInfoList;
            var WeaponInfoList = Const.WeaponInfoList;
            var json = File.ReadAllText(file);
            var originalList = JsonSerializer.Deserialize<List<WishData>>(json, Const.JsonOptions);
            if (originalList == null || originalList.Count == 0)
            {
                return null;
            }
            var groups = originalList.GroupBy(x => x.WishType);
            var resultList = new List<WishData>(originalList.Count);
            foreach (var group in groups)
            {
                var tempList = group.OrderBy(x => x.Id).ToList();
                int tmp = -1;
                bool is_DaBaoDi = false;
                for (int i = 0; i < tempList.Count(); i++)
                {
                    var data = tempList[i];
                    data.Guarantee = i - tmp;
                    data.Number = i;
                    data.GuaranteeType = is_DaBaoDi ? "大保底" : "小保底";
                    if (data.WishType == WishType.Permanent || data.WishType == WishType.Novice)
                    {
                        data.GuaranteeType = "保底内";
                    }
                    if (data.Rank == 5)
                    {
                        tmp = i;
                        if (data.WishType == WishType.CharacterEvent || data.WishType == WishType.WeaponEvent)
                        {
                            var wishevent = WishEventList.Find(x =>
                                x.WishType == data.WishType
                                && x.StartTime <= data.Time
                                && x.EndTime >= data.Time);
                            if (wishevent.UpStar5.Contains(data.Name))
                            {
                                is_DaBaoDi = false;
                            }
                            else
                            {
                                is_DaBaoDi = true;
                            }
                        }
                    }
                    if (data.Rank == 5 || data.Rank == 4)
                    {
                        if (data.WishType == WishType.CharacterEvent || data.WishType == WishType.WeaponEvent)
                        {
                            var wishevent = WishEventList.Find(x =>
                                x.WishType == data.WishType
                                && x.StartTime <= data.Time
                                && x.EndTime >= data.Time);
                            if (wishevent.UpStar5.Contains(data.Name) || wishevent.UpStar4.Contains(data.Name))
                            {
                                data.IsUp = true;
                            }
                            else
                            {
                                data.IsUp = false;
                            }
                        }
                    }
                }
                resultList.AddRange(tempList);
            }
            return resultList.OrderBy(x => x.Id).ToList();
        }
    }
}
