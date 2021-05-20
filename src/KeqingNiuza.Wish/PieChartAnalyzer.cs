using System;
using System.Collections.Generic;
using System.Linq;

namespace KeqingNiuza.Wish
{
    public class PieChartAnalyzer
    {
        List<WishData> WishDatas;
        List<WishData> NoviceDatas = new List<WishData>(20);
        List<WishData> PermanentDatas = new List<WishData>();
        List<WishData> CharacterEventDatas = new List<WishData>();
        List<WishData> WeaponEventDatas = new List<WishData>();

        /// <summary>
        /// 新手祈愿统计
        /// </summary>
        public WishStatistics NoviceStatistics { get; private set; } = new WishStatistics();

        /// <summary>
        /// 常驻祈愿统计
        /// </summary>
        public WishStatistics PermanentStatistics { get; private set; } = new WishStatistics();

        /// <summary>
        /// 角色活动祈愿统计
        /// </summary>
        public WishStatistics CharacterEventStatistics { get; private set; } = new WishStatistics();

        /// <summary>
        /// 武器活动祈愿统计
        /// </summary>
        public WishStatistics WeaponEventStatistics { get; private set; } = new WishStatistics();

        public PieChartAnalyzer(List<WishData> wishDatas)
        {
            if (wishDatas == null || wishDatas.Count == 0)
            {
                throw new ArgumentNullException();
            }
            WishDatas = new List<WishData>(wishDatas.Count);
            // 去重
            WishDatas = wishDatas.Distinct().ToList();

            // 分类
            foreach (var item in WishDatas)
            {
                switch (item.WishType)
                {
                    case WishType.Novice:
                        NoviceDatas.Add(item);
                        break;
                    case WishType.Permanent:
                        PermanentDatas.Add(item);
                        break;
                    case WishType.CharacterEvent:
                        CharacterEventDatas.Add(item);
                        break;
                    case WishType.WeaponEvent:
                        WeaponEventDatas.Add(item);
                        break;
                }
            }

            // 排序
            NoviceDatas = NoviceDatas.OrderBy(x => x.Id).ToList();
            PermanentDatas = PermanentDatas.OrderBy(x => x.Id).ToList();
            CharacterEventDatas = CharacterEventDatas.OrderBy(x => x.Id).ToList();
            WeaponEventDatas = WeaponEventDatas.OrderBy(x => x.Id).ToList();

            // 分析
            Analyze(NoviceDatas, NoviceStatistics);
            Analyze(PermanentDatas, PermanentStatistics);
            Analyze(CharacterEventDatas, CharacterEventStatistics);
            Analyze(WeaponEventDatas, WeaponEventStatistics);
        }

        private void Analyze(List<WishData> datas, WishStatistics statistics)
        {
            if (datas.Count == 0)
            {
                return;
            }
            statistics.WishType = datas[0].WishType;
            statistics.StartTime = datas.Min(x => x.Time);
            statistics.EndTime = datas.Max(x => x.Time);
            statistics.Count = datas.Count;
            statistics.Guarantee = GetGuarantee(datas);
            statistics.GuaranteeType = GetGuaranteeType(datas);
            statistics.Star5List = GetDetailList(datas, 5);
            DefineColor(statistics.Star5List);
            statistics.Star5Count = statistics.Star5List.Count;
            statistics.Star4List = GetDetailList(datas, 4);
            DefineColor(statistics.Star4List);
            statistics.Star4Count = statistics.Star4List.Count;
            statistics.Character5Count = datas.Where(x => x.Rank == 5 && x.ItemType == "角色").Count();
            statistics.Weapon5Count = datas.Where(x => x.Rank == 5 && x.ItemType == "武器").Count();
            statistics.Character4Count = datas.Where(x => x.Rank == 4 && x.ItemType == "角色").Count();
            statistics.Weapon4Count = datas.Where(x => x.Rank == 4 && x.ItemType == "武器").Count();
            statistics.Star3Count = datas.Where(x => x.Rank == 3).Count();
            statistics.Weapon3Count = statistics.Star3Count;
        }


        private static void DefineColor(List<StarDetail> list)
        {
            var random = new Random();
            var brushList = Const.BrushList.OrderBy(x => random.Next()).ToList();
            int brushIndex = 0;
            var groups = list.GroupBy(x => x.Name);
            foreach (var group in groups)
            {
                if (brushIndex >= brushList.Count)
                {
                    brushIndex = 0;
                    brushList = Const.BrushList.OrderBy(x => random.Next()).ToList();
                }
                group.ToList().ForEach(x => x.Brush = brushList[brushIndex]);
                brushIndex++;
            }
        }


        private int GetGuarantee(List<WishData> list)
        {
            if (list.Count == 0)
            {
                return -1;
            }
            var index = list.FindLastIndex(x => x.Rank == 5);
            if (index == -1)
            {
                return list.Count;
            }
            else
            {
                return list.Count - index - 1;
            }
        }


        private string GetGuaranteeType(List<WishData> list)
        {
            if (list.Count == 0)
            {
                return "保底内";
            }
            if (list[0].WishType == WishType.Novice || list[0].WishType == WishType.Permanent)
            {
                return "保底内";
            }
            var index = list.FindLastIndex(x => x.Rank == 5);
            if (index == -1)
            {
                return "小保底";
            }
            else
            {
                var time = list[index].Time;
                var name = list[index].Name;
                var type = list[index].WishType;
                var wishEvent = Const.WishEventList.Find(x => x.WishType == type && x.StartTime <= time && x.EndTime >= time);
                if (wishEvent.UpStar5.Contains(name))
                {
                    return "小保底";
                }
                else
                {
                    return "大保底";
                }
            }
        }


        private List<StarDetail> GetDetailList(List<WishData> datas, int star)
        {
            List<StarDetail> result;
            var list = datas.Where(x => x.Rank == star).Select(x => new StarDetail(x.Name, datas.IndexOf(x), x.Time.ToString("yyyy/MM/dd hh:mm:ss"))).ToList();
            if (list.Any())
            {
                result = new List<StarDetail>(list.Count);
                for (int i = 1; i < list.Count; i++)
                {
                    result.Add(new StarDetail(list[i].Name, list[i].Num - list[i - 1].Num, list[i].Time));
                }
                result = result.Prepend(new StarDetail(list[0].Name, list[0].Num + 1, list[0].Time)).ToList();
            }
            else
            {
                result = new List<StarDetail>();
            }
            return result;
        }

        public void ExportExcelFile(string path)
        {
            var excel = new ExcelExpoter();
            excel.AddGachaData(NoviceDatas);
            excel.AddGachaData(PermanentDatas);
            excel.AddGachaData(CharacterEventDatas);
            excel.AddGachaData(WeaponEventDatas);
            excel.SaveAs(path);
        }

    }
}
