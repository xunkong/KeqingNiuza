using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KeqingNiuza.Core.Wish
{
    public class WishSummary : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public List<WishData> WishDataList { get; set; }

        private WishStatistics _CharacterStatistics;
        public WishStatistics CharacterStatistics
        {
            get { return _CharacterStatistics; }
            set
            {
                _CharacterStatistics = value;
                OnPropertyChanged();
            }
        }

        private WishStatistics _WeaponStatistics;
        public WishStatistics WeaponStatistics
        {
            get { return _WeaponStatistics; }
            set
            {
                _WeaponStatistics = value;
                OnPropertyChanged();
            }
        }


        private WishStatistics _PermanentStatistics;
        public WishStatistics PermanentStatistics
        {
            get { return _PermanentStatistics; }
            set
            {
                _PermanentStatistics = value;
                OnPropertyChanged();
            }
        }


        public List<ItemInfo> CharacterInfoList { get; set; }

        public List<ItemInfo> WeaponInfoList { get; set; }


        private static Random Random = new Random();

        public static WishSummary Create(List<WishData> list, bool ignoreFirstStar5Character = false, bool ignoreFirstStar5Weapon = false, bool ignoreFirstStar5Permanent = false)
        {
            WishSummary summary = new WishSummary
            {
                WishDataList = list
            };
            var sublist = list.Where(x => x.WishType == WishType.CharacterEvent || x.WishType == WishType.CharacterEvent_2).OrderBy(x => x.Id).ToList();
            if (sublist.Any())
            {
                summary.CharacterStatistics = GetStatistics(sublist, ignoreFirstStar5Character);
            }
            sublist = list.Where(x => x.WishType == WishType.WeaponEvent).OrderBy(x => x.Id).ToList();
            if (sublist.Any())
            {
                summary.WeaponStatistics = GetStatistics(sublist, ignoreFirstStar5Weapon);
            }
            sublist = list.Where(x => x.WishType == WishType.Permanent).OrderBy(x => x.Id).ToList();
            if (sublist.Any())
            {
                summary.PermanentStatistics = GetStatistics(sublist, ignoreFirstStar5Permanent);
            }
            summary.CharacterInfoList = GetCharacterInfoList(list);
            summary.WeaponInfoList = GetWeaponInfoList(list);
            return summary;
        }


        public static WishStatistics GetStatistics(List<WishData> list, bool ignoreFirstStar5 = false)
        {
            var first5 = list.FirstOrDefault(x => x.Rank == 5);
            if (ignoreFirstStar5)
            {
                var i = list.FindIndex(x => x.Rank == 5);
                list = list.Skip(i + 1).ToList();
            }
            if (list.Count == 0)
            {
                return null;
            }

            WishStatistics ws = new WishStatistics
            {
                WishType = list[0].WishType,
                StartTime = list.Min(x => x.Time),
                EndTime = list.Max(x => x.Time),
                Count = list.Count
            };
            if (list.Last().Rank == 5)
            {
                ws.Guarantee = 0;
            }
            else
            {
                ws.Guarantee = list.Last().Guarantee;
            }
            ws.GuaranteeType = list.Last().GuaranteeType;
            ws.Count_XiaoBaoDi = list.Count(x => (x.GuaranteeType == "小保底" || x.GuaranteeType == "保底内") && x.Rank == 5);
            ws.Count_DaBaoDi = list.Count(x => x.GuaranteeType == "大保底" && x.Rank == 5);
            ws.Star5List = GetDetailList(list, 5);
            DefineColor(ws.Star5List);
            ws.Star5Count = ws.Star5List.Count;
            ws.Star4List = GetDetailList(list, 4);
            DefineColor(ws.Star4List);
            ws.Star4Count = ws.Star4List.Count;
            ws.Character5Count = list.Where(x => x.Rank == 5 && x.ItemType == "角色").Count();
            ws.Weapon5Count = list.Where(x => x.Rank == 5 && x.ItemType == "武器").Count();
            ws.Character4Count = list.Where(x => x.Rank == 4 && x.ItemType == "角色").Count();
            ws.Weapon4Count = list.Where(x => x.Rank == 4 && x.ItemType == "武器").Count();
            ws.Star3Count = list.Where(x => x.Rank == 3).Count();
            ws.Weapon3Count = ws.Star3Count;
            var star5list = list.Where(x => x.Rank == 5);
            if (ws.Star5List.Any())
            {
                ws.GuaranteeMax = ws.Star5List.Max(x => x.Num);
                ws.GuaranteeMaxItems = ws.Star5List.Where(x => x.Num == ws.GuaranteeMax).ToList();
                ws.GuaranteeMin = ws.Star5List.Min(x => x.Num);
                ws.GuaranteeMinItems = ws.Star5List.Where(x => x.Num == ws.GuaranteeMin).ToList();
            }
            else
            {
                ws.GuaranteeMax = ws.Count;
                ws.GuaranteeMin = ws.Count;
            }
            var index = list.FindLastIndex(x => x.Rank == 5 && (x.IsUp ?? false));
            if (index == -1)
            {
                ws.AverageUp5 = double.NaN;
            }
            else
            {
                ws.AverageUp5 = (double)(index + 1) / list.Count(x => x.Rank == 5 && (x.IsUp ?? false));
            }

            // Github@Yinr
            // https://github.com/Yinr/KeqingNiuza/commit/5a684dbedb699d2d056631072567ff8b235ba613
            if (ignoreFirstStar5 && first5 != null)
            {
                var detail = new StarDetail
                {
                    Name = first5.Name,
                    Num = first5.Number + 1,
                    Brush = "#ACACAC",
                    Time = $"{first5.Time:yyyy/MM/dd HH:mm:ss} (不计入统计)"
                };
                ws.Star5List.Insert(0, detail);
            }
            return ws;
        }


        private static void DefineColor(List<StarDetail> list)
        {
            var brushList = Const.BrushList.OrderBy(x => Random.Next()).ToList();
            int brushIndex = 0;
            var groups = list.GroupBy(x => x.Name);
            foreach (var group in groups)
            {
                if (brushIndex >= brushList.Count)
                {
                    brushIndex = 0;
                    brushList = Const.BrushList.OrderBy(x => Random.Next()).ToList();
                }
                group.ToList().ForEach(x => x.Brush = brushList[brushIndex]);
                brushIndex++;
            }
        }


        private static void DefineColor(List<StarDetail> list, bool ignoreFirstStar5)
        {
            var brushList = Const.BrushList.OrderBy(x => Random.Next()).ToList();
            if (list.Count == 0)
            {
                return;
            }
            if (list.Count == 1 && ignoreFirstStar5)
            {
                list[0].Brush = "LightGray";
                return;
            }
            if (ignoreFirstStar5)
            {
                list[0].Brush = "LightGray";
            }
            int brushIndex = 0;
            var groups = list.SkipWhile(x => x.Brush != null).GroupBy(x => x.Name);
            foreach (var group in groups)
            {
                if (brushIndex >= brushList.Count)
                {
                    brushIndex = 0;
                    brushList = Const.BrushList.OrderBy(x => Random.Next()).ToList();
                }
                group.ToList().ForEach(x => x.Brush = brushList[brushIndex]);
                brushIndex++;
            }
        }



        private static List<ItemInfo> GetCharacterInfoList(List<WishData> list)
        {
            var groups = Const.CharacterInfoList.GroupJoin(list, x => x.Name, y => y.Name, (x, y) =>
            {
                x.WishDataList = y.Where(y1 => x.Name == y1.Name).OrderBy(y1 => y1.Id).ToList();
                x.Count = x.WishDataList.Count;
                //todo 可能有问题
                x.LastGetTime = x.WishDataList.LastOrDefault()?.Time ?? new DateTime();
                return x;
            });
            return groups.Where(x => x.Count > 0).Select(x => x as ItemInfo).ToList();
        }

        private static List<ItemInfo> GetWeaponInfoList(List<WishData> list)
        {
            var groups = Const.WeaponInfoList.GroupJoin(list, x => x.Name, y => y.Name, (x, y) =>
            {
                x.WishDataList = y.Where(y1 => x.Name == y1.Name).OrderBy(y1 => y1.Id).ToList();
                x.Count = x.WishDataList.Count;
                //todo 可能有问题
                x.LastGetTime = x.WishDataList.LastOrDefault()?.Time ?? new DateTime();
                return x;
            });
            return groups.Where(x => x.Count > 0).Select(x => x as ItemInfo).ToList();
        }

        private static List<StarDetail> GetDetailList(List<WishData> datas, int star)
        {
            List<StarDetail> result;
            var list = datas.Where(x => x.Rank == star).Select(x => new StarDetail(x.Name, datas.IndexOf(x), x.Time.ToString("yyyy/MM/dd HH:mm:ss"))).ToList();
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

    }
}
