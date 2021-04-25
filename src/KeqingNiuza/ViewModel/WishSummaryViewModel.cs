using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeqingNiuza.Wish;
using KeqingNiuza.Model;
using System.Text.Json;
using static KeqingNiuza.Common.Const;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace KeqingNiuza.ViewModel
{
    public class WishSummaryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




        private SolidColorBrush BrushCharacter5 { get; } = new SolidColorBrush(new Color { R = 0xFA, G = 0xC8, B = 0x58, A = 0xFF });
        private SolidColorBrush BrushWeapon5 { get; } = new SolidColorBrush(new Color { R = 0xEE, G = 0x66, B = 0x66, A = 0xFF });
        private SolidColorBrush BrushCharacter4 { get; } = new SolidColorBrush(new Color { R = 0x54, G = 0x70, B = 0xC6, A = 0xFF });
        private SolidColorBrush BrushWeapon4 { get; } = new SolidColorBrush(new Color { R = 0x91, G = 0xCC, B = 0x75, A = 0xFF });
        private SolidColorBrush BrushWeapon3 { get; } = new SolidColorBrush(new Color { R = 0x73, G = 0xC0, B = 0xDE, A = 0xFF });

        public WishSummaryViewModel(UserData userData)
        {
            var json = File.ReadAllText(userData.WishLogFile);
            var list = JsonSerializer.Deserialize<List<WishData>>(json, JsonOptions);
            var analyzer = new WishAnalyzer(list);
            NoviceStatistics = analyzer.NoviceStatistics;
            PermanentStatistics = analyzer.PermanentStatistics;
            CharacterEventStatistics = analyzer.CharacterEventStatistics;
            WeaponEventStatistics = analyzer.WeaponEventStatistics;
            InitPieChart();
        }


        public WishStatistics NoviceStatistics { get; set; }
        public WishStatistics PermanentStatistics { get; set; }
        public WishStatistics CharacterEventStatistics { get; set; }
        public WishStatistics WeaponEventStatistics { get; set; }

        public SeriesCollection NoviceSeries { get; set; }
        public SeriesCollection PermanentSeries { get; set; }
        public SeriesCollection CharacterEventSeries { get; set; }
        public SeriesCollection WeaponEventSeries { get; set; }


        private void InitPieChart()
        {
            PieSeries series;

            // Novice
            NoviceSeries = new SeriesCollection(3);
            series = new PieSeries()
            {
                Title = "5星角色",
                Fill = BrushCharacter5,
                Values = new ChartValues<int> { 0, NoviceStatistics.Character5Count }
            };
            NoviceSeries.Add(series);
            series = new PieSeries()
            {
                Title = "4星角色",
                Fill = BrushCharacter4,
                Values = new ChartValues<int> { 0, NoviceStatistics.Character4Count }
            };
            NoviceSeries.Add(series);
            series = new PieSeries()
            {
                Title = "3星武器",
                Fill = BrushWeapon3,
                Values = new ChartValues<int> { 0, NoviceStatistics.Weapon3Count }
            };
            NoviceSeries.Add(series);

            // Permanent
            PermanentSeries = new SeriesCollection(4);
            series = new PieSeries()
            {
                Title = "5星角色",
                Fill = BrushCharacter5,
                Values = new ChartValues<int> { 0, PermanentStatistics.Character5Count }
            };
            PermanentSeries.Add(series);
            series = new PieSeries()
            {
                Title = "4星角色",
                Fill = BrushCharacter4,
                Values = new ChartValues<int> { 0, PermanentStatistics.Character4Count }
            };
            PermanentSeries.Add(series);
            series = new PieSeries()
            {
                Title = "5星武器",
                Fill = BrushWeapon5,
                Values = new ChartValues<int> { 0, PermanentStatistics.Weapon5Count }
            };
            PermanentSeries.Add(series);
            series = new PieSeries()
            {
                Title = "4星武器",
                Fill = BrushWeapon4,
                Values = new ChartValues<int> { 0, PermanentStatistics.Weapon4Count }
            };
            PermanentSeries.Add(series);

            // CharacterEvent
            CharacterEventSeries = new SeriesCollection(3);
            series = new PieSeries()
            {
                Title = "5星角色",
                Fill = BrushCharacter5,
                Values = new ChartValues<int> { 0, CharacterEventStatistics.Character5Count }
            };
            CharacterEventSeries.Add(series);
            series = new PieSeries()
            {
                Title = "4星角色",
                Fill = BrushCharacter4,
                Values = new ChartValues<int> { 0, CharacterEventStatistics.Character4Count }
            };
            CharacterEventSeries.Add(series);
            series = new PieSeries()
            {
                Title = "4星武器",
                Fill = BrushWeapon4,
                Values = new ChartValues<int> { 0, CharacterEventStatistics.Weapon4Count }
            };
            CharacterEventSeries.Add(series);

            // WeaponEvent
            WeaponEventSeries = new SeriesCollection(3);
            series = new PieSeries()
            {
                Title = "4星角色",
                Fill = BrushCharacter4,
                Values = new ChartValues<int> { 0, WeaponEventStatistics.Character4Count }
            };
            WeaponEventSeries.Add(series);
            series = new PieSeries()
            {
                Title = "5星武器",
                Fill = BrushWeapon5,
                Values = new ChartValues<int> { 0, WeaponEventStatistics.Weapon5Count }
            };
            WeaponEventSeries.Add(series);
            series = new PieSeries()
            {
                Title = "4星武器",
                Fill = BrushWeapon4,
                Values = new ChartValues<int> { 0, WeaponEventStatistics.Weapon4Count }
            };
            WeaponEventSeries.Add(series);
        }

    }
}
