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
            var json = File.ReadAllText(file);
            var originalList = JsonSerializer.Deserialize<List<WishData>>(json, Const.JsonOptions);
            var groups = originalList.GroupBy(x => x.WishType);
            var resultList = new List<WishData>(originalList.Count);
            foreach (var group in groups)
            {
                var tempList = group.OrderBy(x => x.Id).ToList();
                int tmp = -1;
                for (int i = 0; i < tempList.Count(); i++)
                {
                    tempList[i].Guarantee = i - tmp;
                    tempList[i].Number = i;
                    if (tempList[i].Rank == 5)
                    {
                        tmp = i;
                    }
                }
                resultList.AddRange(tempList);
            }
            return resultList.OrderBy(x => x.Id).ToList();
        }
    }
}
