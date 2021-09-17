using System;
using System.Collections.Generic;
using System.Linq;

namespace KeqingNiuza.Core.Wish
{
    /// <summary>
    /// 成就计算方法
    /// </summary>
    static class AchievementComputeMethod
    {

        private static List<WishEvent> WishEventList;

        static AchievementComputeMethod()
        {
            WishEventList = Const.WishEventList;
        }

        #region 欧非


        /// <summary>
        /// 85抽之后才出5星
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 非酋竟是我自己(List<WishData> datas, List<AchievementInfo> infos)
        {
            var list = datas.OrderByDescending(x => x.Guarantee).ThenBy(x => x.Time);
            var a = list.First();
            if (a.Guarantee > 84)
            {
                infos.Add(new AchievementInfo
                {
                    Name = $"非酋竟是我自己",
                    Description = $"85抽之后才出5星",
                    IsFinished = true,
                    FinishTime = a.Time,
                    Total = $"总计 {a.Guarantee}"
                });
            }
        }


        /// <summary>
        /// 十连两金及以上
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 欧皇时刻(List<WishData> datas, List<AchievementInfo> infos)
        {
            var list = datas.GroupBy(x => x.Time).Where(g => g.Count(x => x.Rank == 5) >= 2).OrderBy(g => g.Key);
            if (list.Any())
            {
                var info = new AchievementInfo
                {
                    Name = "欧皇时刻！",
                    Description = "十连两金及以上",
                    IsFinished = true,
                    FinishTime = list.First().Key,
                    Total = $"总计 {list.Count()} 次",
                };
                infos.Add(info);
            }
        }



        #endregion




        #region 最高记录


        /// <summary>
        /// 抽到最多的5星角色
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        private static void 五星角色爱你哟(List<WishData> datas, List<AchievementInfo> infos)
        {
            var groups = datas.Where(x => x.ItemType == "角色" && x.Rank == 5).GroupBy(x => x.Name);
            groups = groups.OrderByDescending(x => x.Count());
            var n = groups.First().Count();
            foreach (var group in groups)
            {
                if (group.Count() == n)
                {
                    var list = group.OrderByDescending(x => x.Time);
                    var a = list.First();
                    var info = new AchievementInfo
                    {
                        Name = $"「{a.Name}」爱你哟",
                        Description = $"抽到最多的5星角色是「{a.Name}」",
                        IsFinished = true,
                        FinishTime = a.Time,
                        Total = $"总计 {list.Count()}",
                    };
                    infos.Add(info);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 抽到最多的4星角色
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        private static void 四星角色爱你哟(List<WishData> datas, List<AchievementInfo> infos)
        {
            var groups = datas.Where(x => x.ItemType == "角色" && x.Rank == 4).GroupBy(x => x.Name);
            groups = groups.OrderByDescending(x => x.Count());
            var n = groups.First().Count();
            foreach (var group in groups)
            {
                if (group.Count() == n)
                {
                    var list = group.OrderByDescending(x => x.Time);
                    var a = list.First();
                    var info = new AchievementInfo
                    {
                        Name = $"「{a.Name}」爱你哟",
                        Description = $"抽到最多的4星角色是「{a.Name}」",
                        IsFinished = true,
                        FinishTime = a.Time,
                        Total = $"总计 {list.Count()}"
                    };
                    infos.Add(info);
                }
                else
                {
                    break;
                }
            }
        }


        /// <summary>
        /// 一天抽卡达到78次
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 一掷千金(List<WishData> datas, List<AchievementInfo> infos)
        {
            var group = datas.GroupBy(x => x.Time.Date).OrderByDescending(x => x.Count()).First();
            if (group.Count() >= 78)
            {
                var info = new AchievementInfo
                {
                    Name = "一掷千金",
                    Description = "一天抽卡达到78次",
                    Comment = "千金指1000人民币",
                    IsFinished = true,
                    FinishTime = group.Key,
                    Total = $"总计 {group.Count()}",
                };
                infos.Add(info);
            }
            else
            {
                var info = new AchievementInfo
                {
                    Name = "一掷千金",
                    Description = "一天抽卡达到78次",
                    IsFinished = false,
                    Progress = $"{group.Count()} / 78",
                };
                infos.Add(info);
            }
        }


        /// <summary>
        /// 累计20天没有进行活动祈愿
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 仓鼠(List<WishData> datas, List<AchievementInfo> infos)
        {
            var list = datas.Where(x => x.WishType == WishType.CharacterEvent || x.WishType == WishType.WeaponEvent).GroupBy(x => x.Time.Date).OrderBy(g => g.Key).ToList();
            if (list.Count <= 2)
            {
                return;
            }
            var span = list[1].Key - list[0].Key;
            var time = list[1].Key;
            for (int i = 1; i < list.Count; i++)
            {
                var tmp = list[i].Key - list[i - 1].Key;
                if (tmp > span)
                {
                    span = tmp;
                    time = list[i].Key;
                }
            }
            var info = new AchievementInfo
            {
                Name = "仓鼠",
                Description = "累计超过20天没有进行活动祈愿",
                Comment = "原石的数量，令人安心",
                IsFinished = true,
                FinishTime = time,
                Total = $"总计 {span.Days - 1}",
            };
            infos.Add(info);
        }


        #endregion



        #region 满命







        #endregion



        #region 其他


        /// <summary>
        /// 在一次「赤团开时」活动祈愿中获取三个胡桃
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 三个胡桃(List<WishData> datas, List<AchievementInfo> infos)
        {
            var events = WishEventList.FindAll(x => x.Name == "赤团开时");
            foreach (var item in events)
            {
                var list = datas.Where(x => x.Time >= item.StartTime && x.Time <= item.EndTime && x.WishType == WishType.CharacterEvent).Select(x => x);
                list = list.Where(x => x.Name == "胡桃").Select(x => x);
                if (list.Count() >= 3)
                {
                    var info = new AchievementInfo
                    {
                        Name = "一个胡桃，两个胡桃，三个胡桃",
                        Description = "在一次「赤团开时」活动祈愿中获取三个胡桃",
                        IsFinished = true,
                        FinishTime = list.ElementAt(2).Time,
                        Total = $"总计 {list.Count()}",
                    };
                    infos.Add(info);
                }
            }

        }


        /// <summary>
        /// 在一次「浮生孰来」活动祈愿中抽中「甘雨」和「七七」
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 椰羊的奶好喝(List<WishData> datas, List<AchievementInfo> infos)
        {
            var events = WishEventList.FindAll(x => x.Name == "浮生孰来");
            foreach (var item in events)
            {
                var list = datas.Where(x => x.Time >= item.StartTime && x.Time <= item.EndTime && x.WishType == WishType.CharacterEvent);
                list = list.Where(x => x.Name == "甘雨" || x.Name == "七七").Select(x => x);
                bool hasGanyu = false, hasQiqi = false;
                foreach (var data in list)
                {
                    if (data.Name == "甘雨")
                    {
                        hasGanyu = true;
                    }
                    else
                    {
                        hasQiqi = true;
                    }
                    if (hasGanyu && hasQiqi)
                    {
                        var info = new AchievementInfo
                        {
                            Name = "「椰羊的奶，好喝！」",
                            Description = "在一次「浮生孰来」活动祈愿中抽中「甘雨」和「七七」",
                            IsFinished = true,
                            FinishTime = data.Time,
                        };
                        infos.Add(info);
                        break;
                    }
                }
            }

        }


        /// <summary>
        /// 累计抽出7把狼的末路
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 七匹狼(List<WishData> datas, List<AchievementInfo> infos)
        {
            var list = datas.Where(x => x.Name == "狼的末路");
            if (list.Count() >= 7)
            {
                var info = new AchievementInfo
                {
                    Name = "七匹狼",
                    Description = "累计抽出7把狼的末路",
                    IsFinished = true,
                    FinishTime = list.ElementAt(6).Time,
                    Total = $"总计 {list.Count()}",
                };
                infos.Add(info);
            }

        }


        /// <summary>
        /// 在一次「神铸赋形」活动祈愿中抽出7把狼的末路，还没有抽出护摩之杖
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 七匹狼的诅咒(List<WishData> datas, List<AchievementInfo> infos)
        {
            var events = WishEventList.Where(x => x.UpStar5.Contains("护摩之杖"));
            foreach (var item in events)
            {
                var list = datas.Where(x => x.Time >= item.StartTime && x.Time <= item.EndTime && x.WishType == WishType.WeaponEvent).Where(x => x.Name == "狼的末路" || x.Name == "护摩之杖");
                int langmo = 0;
                DateTime time = DateTime.Now;
                foreach (var data in list)
                {
                    if (data.Name == "狼的末路")
                    {
                        langmo++;
                        time = data.Time;
                    }
                    else
                    {
                        break;
                    }
                }
                if (langmo >= 7)
                {
                    var info = new AchievementInfo
                    {
                        Name = "七匹狼的诅咒",
                        Description = "在一次「神铸赋形」活动祈愿中抽出7把狼的末路，还没有抽出护摩之杖",
                        IsFinished = true,
                        FinishTime = time,
                        Total = $"总计 {langmo}",
                    };
                    infos.Add(info);
                }
                if (langmo > 0)
                {
                    var info = new AchievementInfo
                    {
                        Name = "七匹狼的诅咒",
                        Description = "在一次「神铸赋形」活动祈愿中抽出7把狼的末路，还没有抽出护摩之杖",
                        IsFinished = false,
                        Progress = $"{langmo} / 7",
                    };
                    infos.Add(info);
                }
            }

        }


        /// <summary>
        /// 在一次「浮生孰来」活动祈愿中抽中「甘雨」和「刻晴」
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 晴深深雨蒙蒙(List<WishData> datas, List<AchievementInfo> infos)
        {
            var events = WishEventList.FindAll(x => x.Name == "浮生孰来");
            foreach (var item in events)
            {
                var list = datas.Where(x => x.Time >= item.StartTime && x.Time <= item.EndTime && x.WishType == WishType.CharacterEvent);
                list = list.Where(x => x.Name == "甘雨" || x.Name == "刻晴").Select(x => x);
                bool hasGanyu = false, hasKeqing = false;
                foreach (var data in list)
                {
                    if (data.Name == "甘雨")
                    {
                        hasGanyu = true;
                    }
                    else
                    {
                        hasKeqing = true;
                    }
                    if (hasGanyu && hasKeqing)
                    {
                        var info = new AchievementInfo
                        {
                            Name = "晴深深雨蒙蒙",
                            Description = "在一次「浮生孰来」活动祈愿中抽中「甘雨」和「刻晴」",
                            IsFinished = true,
                            FinishTime = data.Time,
                        };
                        infos.Add(info);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 在一次「深秘之息」活动祈愿中抽中「阿贝多」和「砂糖」
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 为什么不问问神奇的阿贝多呢(List<WishData> datas, List<AchievementInfo> infos)
        {
            var events = WishEventList.FindAll(x => x.Name == "深秘之息");
            foreach (var item in events)
            {
                var list = datas.Where(x => x.Time >= item.StartTime && x.Time <= item.EndTime && x.WishType == WishType.CharacterEvent);
                list = list.Where(x => x.Name == "阿贝多" || x.Name == "砂糖");
                bool hasAlbedo = false, hasSucrose = false;
                foreach (var data in list)
                {
                    if (data.Name == "阿贝多")
                    {
                        hasAlbedo = true;
                    }
                    else
                    {
                        hasSucrose = true;
                    }
                    if (hasAlbedo && hasSucrose)
                    {
                        var info = new AchievementInfo
                        {
                            Name = "为什么不问问神奇的阿贝多呢",
                            Description = "在一次「深秘之息」活动祈愿中抽中「阿贝多」和「砂糖」",
                            IsFinished = true,
                            FinishTime = data.Time,
                        };
                        infos.Add(info);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 累计抽出8把风鹰剑
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 八重鹰(List<WishData> datas, List<AchievementInfo> infos)
        {
            var list = datas.Where(x => x.Name == "风鹰剑");
            if (list.Count() >= 8)
            {
                var info = new AchievementInfo
                {
                    Name = "风鹰剑",
                    Description = "累计抽出8把风鹰剑",
                    IsFinished = true,
                    FinishTime = list.ElementAt(7).Time,
                    Total = $"总计 {list.Count()}",
                };
                infos.Add(info);
            }

        }


        /// <summary>
        /// 在一次「神铸赋形」活动祈愿中抽出8把风鹰剑，还没有抽出松籁响起之时
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 鹰鹰鹰鹰鹰鹰鹰鹰(List<WishData> datas, List<AchievementInfo> infos)
        {
            var events = WishEventList.Where(x => x.UpStar5.Contains("松籁响起之时"));
            foreach (var item in events)
            {
                var list = datas.Where(x => x.Time >= item.StartTime && x.Time <= item.EndTime && x.WishType == WishType.WeaponEvent).Where(x => x.Name == "风鹰剑" || x.Name == "松籁响起之时");
                int fengying = 0;
                DateTime time = DateTime.Now;
                foreach (var data in list)
                {
                    if (data.Name == "风鹰剑")
                    {
                        fengying++;
                        time = data.Time;
                    }
                    else
                    {
                        break;
                    }
                }
                if (fengying >= 8)
                {
                    var info = new AchievementInfo
                    {
                        Name = "鹰鹰鹰鹰鹰鹰鹰鹰",
                        Description = "在一次「神铸赋形」活动祈愿中抽出8把风鹰剑，还没有抽出松籁响起之时",
                        IsFinished = true,
                        FinishTime = time,
                        Total = $"总计 {fengying}",
                    };
                    infos.Add(info);
                }
                if (fengying > 0)
                {
                    var info = new AchievementInfo
                    {
                        Name = "鹰鹰鹰鹰鹰鹰鹰鹰",
                        Description = "在一次「神铸赋形」活动祈愿中抽出8把风鹰剑，还没有抽出松籁响起之时",
                        IsFinished = false,
                        Progress = $"{fengying} / 8",
                    };
                    infos.Add(info);
                }
            }

        }


        /// <summary>
        /// 没有抽过武器池
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 不存在的武器池(List<WishData> datas, List<AchievementInfo> infos)
        {
            var list = datas.Where(x => x.WishType == WishType.WeaponEvent);
            if (list.Any())
            {
                return;
            }
            var info = new AchievementInfo
            {
                Name = "不存在的武器池",
                Description = "没有抽过武器池",
                Comment = "「我一发都不敢抽啊」",
                IsFinished = true,
                FinishTime = DateTime.Now,
            };
            infos.Add(info);
        }


        /// <summary>
        /// 在活动祈愿关闭前最后一分钟抽到当期Up5星
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="infos"></param>
        public static void 最后一分钟的奇迹(List<WishData> datas, List<AchievementInfo> infos)
        {
            int count1 = 0, count2 = 0, count3 = 0;
            DateTime time1 = new DateTime(), time2 = new DateTime(), time3 = new DateTime();
            foreach (var @event in WishEventList)
            {
                var list = datas.Where(x => x.WishType == @event.WishType && x.Time >= @event.EndTime - new TimeSpan(0, 1, 0) && x.Time <= @event.EndTime);
                if (list.Any())
                {
                    var star5 = list.Where(x => x.Rank == 5);
                    if (star5.Any())
                    {
                        bool isup = false;
                        foreach (var item in star5)
                        {
                            if (@event.UpStar5.Contains(item.Name))
                            {
                                isup = true;
                                count1++;
                                if (time1 == new DateTime())
                                {
                                    time1 = item.Time;
                                }
                                break;
                            }
                        }
                        if (!isup)
                        {
                            count2++;
                            if (time2 == new DateTime())
                            {
                                time2 = star5.First().Time;
                            }
                        }
                    }
                    else
                    {
                        count3++;
                        if (time3 == new DateTime())
                        {
                            time3 = list.First().Time;
                        }
                    }
                }
            }
            if (count1 != 0)
            {
                var info = new AchievementInfo
                {
                    Name = "最后一分钟的奇迹",
                    Description = "在活动祈愿关闭前最后一分钟抽到当期Up5星",
                    Comment = "有惊无险",
                    IsFinished = true,
                    FinishTime = time1,
                    Total = $"总计 {count1}",
                };
                infos.Add(info);
            }
            if (count2 != 0)
            {
                var info = new AchievementInfo
                {
                    Name = "最后一分钟的奇迹？",
                    Description = "在活动祈愿关闭前最后一分钟没抽到当期Up5星",
                    Comment = "抽到了，但没完全抽到",
                    IsFinished = true,
                    FinishTime = time2,
                    Total = $"总计 {count2}",
                };
                infos.Add(info);
            }
            if (count3 != 0)
            {
                var info = new AchievementInfo
                {
                    Name = "最后一分钟没有奇迹",
                    Description = "在活动祈愿关闭前最后一分钟没抽到5星",
                    IsFinished = true,
                    FinishTime = time3,
                    Total = $"总计 {count3}",
                };
                infos.Add(info);
            }

        }



        #endregion


    }
}
