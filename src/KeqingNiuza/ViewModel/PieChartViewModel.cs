using KeqingNiuza.Wish;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows.Media;

namespace KeqingNiuza.ViewModel
{


    [Obsolete("先这么用着，未来会删除")]
    public class PieChartViewModel
    {
        public SolidColorBrush BrushCharacter5 { get; } = new SolidColorBrush(new Color { R = 0xFA, G = 0xC8, B = 0x58, A = 0xFF });
        public SolidColorBrush BrushWeapon5 { get; } = new SolidColorBrush(new Color { R = 0xEE, G = 0x66, B = 0x66, A = 0xFF });
        public SolidColorBrush BrushCharacter4 { get; } = new SolidColorBrush(new Color { R = 0x54, G = 0x70, B = 0xC6, A = 0xFF });
        public SolidColorBrush BrushWeapon4 { get; } = new SolidColorBrush(new Color { R = 0x91, G = 0xCC, B = 0x75, A = 0xFF });
        public SolidColorBrush BrushWeapon3 { get; } = new SolidColorBrush(new Color { R = 0x73, G = 0xC0, B = 0xDE, A = 0xFF });


        public PieChartViewModel(WishStatistics statistics)
        {
            if (statistics == null || statistics.Count == 0)
            {
                return;
            }
            Statistics = statistics;
            Series = new SeriesCollection();
            InitData();
            InitPieChart();
        }


        public WishStatistics Statistics { get; set; }
        public string Title { get; set; }
        public SeriesCollection Series { get; set; }
        public PieSeries SeriesCharacter5 { get; set; }
        public PieSeries SeriesWeapon5 { get; set; }
        public PieSeries SeriesCharacter4 { get; set; }
        public PieSeries SeriesWeapon4 { get; set; }
        public PieSeries SeriesWeapon3 { get; set; }
        public string StartEndTime { get; set; }
        public string Star5Count { get; set; }
        public string Star4Count { get; set; }
        public string Star3Count { get; set; }
        public string Star5Ratio { get; set; }
        public string Star4Ratio { get; set; }
        public string Star3Ratio { get; set; }
        public string Average5 { get; set; }


        private void InitData()
        {
            switch (Statistics.WishType)
            {
                case WishType.Novice:
                    Title = "新手祈愿";
                    break;
                case WishType.Permanent:
                    Title = "常驻祈愿";
                    break;
                case WishType.CharacterEvent:
                    Title = "角色活动祈愿";
                    break;
                case WishType.WeaponEvent:
                    Title = "武器活动祈愿";
                    break;
            }
            StartEndTime = $"{Statistics.StartTime:yyyy/MM/dd hh:mm:ss}  -  {Statistics.EndTime:yyyy/MM/dd hh:mm:ss}";
            Star5Count = Statistics.Star5Count.ToString().PadRight(5, ' ');
            Star4Count = Statistics.Star4Count.ToString().PadRight(5, ' ');
            Star3Count = Statistics.Star3Count.ToString().PadRight(5, ' ');
            Star5Ratio = $"[{Statistics.Ratio5:P3}]";
            Star4Ratio = $"[{Statistics.Ratio4:P3}]";
            Star3Ratio = $"[{Statistics.Ratio3:P3}]";
            Average5 = Statistics.Average5.ToString("F2");
        }

        private void InitPieChart()
        {
            if (Statistics.Character5Count > 0)
            {
                SeriesCharacter5 = new PieSeries()
                {
                    Title = "5星角色",
                    Fill = BrushCharacter5,
                    Values = new ChartValues<int> { Statistics.Character5Count }
                };
                Series.Add(SeriesCharacter5);

            }
            if (Statistics.Weapon5Count > 0)
            {
                SeriesWeapon5 = new PieSeries()
                {
                    Title = "5星武器",
                    Fill = BrushWeapon5,
                    Values = new ChartValues<int> { Statistics.Weapon5Count }
                };
                Series.Add(SeriesWeapon5);

            }
            if (Statistics.Character4Count > 0)
            {
                SeriesCharacter4 = new PieSeries()
                {
                    Title = "4星角色",
                    Fill = BrushCharacter4,
                    Values = new ChartValues<int> { Statistics.Character4Count }
                };
                Series.Add(SeriesCharacter4);

            }
            if (Statistics.Weapon4Count > 0)
            {
                SeriesWeapon4 = new PieSeries()
                {
                    Title = "4星武器",
                    Fill = BrushWeapon4,
                    Values = new ChartValues<int> { Statistics.Weapon4Count }
                };
                Series.Add(SeriesWeapon4);
            }
            if (Statistics.Weapon3Count > 0)
            {
                SeriesWeapon3 = new PieSeries()
                {
                    Title = "3星武器",
                    Fill = BrushWeapon3,
                    Values = new ChartValues<int> { Statistics.Weapon3Count }
                };
                if (Statistics.WishType == WishType.Novice)
                {
                    Series.Add(SeriesWeapon3);
                }
            }

        }

    }
}
