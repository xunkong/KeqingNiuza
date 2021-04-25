using GenshinHelper.Desktop.ViewModel;
using KeqingNiuza.Model;
using KeqingNiuza.Wish;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System;
using static KeqingNiuza.Common.Const;

namespace GenshinHelper.Desktop.View
{
    /// <summary>
    /// GachaAnalysisView.xaml 的交互逻辑
    /// </summary>
    [Obsolete("先这么用着，未来会删除")]
    public partial class GachaAnalysisView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public GachaAnalysisView()
        {
            InitializeComponent();
            _PieChartViewModels = new ObservableCollection<PieChartViewModel>();
            ItemsControl_PieChartView.DataContext = this;
        }

        public GachaAnalysisView(UserData userData)
        {
            InitializeComponent();
            _UserData = userData;
            ToggleButton_HiddenNoviceWish.IsChecked = userData.HiddenNoviceWish;
            _PieChartViewModels = new ObservableCollection<PieChartViewModel>();
            ItemsControl_PieChartView.DataContext = this;
            var gachaList = JsonSerializer.Deserialize<List<WishData>>(File.ReadAllText(userData.WishLogFile), JsonOptions);
            var analyzer = new WishAnalyzer(gachaList);
            PieChartViewModels.Clear();

            if (analyzer.CharacterEventStatistics.Count > 0)
            {
                PieChartViewModels.Add(new PieChartViewModel(analyzer.CharacterEventStatistics));
            }
            if (analyzer.WeaponEventStatistics.Count > 0)
            {
                PieChartViewModels.Add(new PieChartViewModel(analyzer.WeaponEventStatistics));
            }
            if (analyzer.PermanentStatistics.Count > 0)
            {
                PieChartViewModels.Add(new PieChartViewModel(analyzer.PermanentStatistics));
            }
            _NovicePieChart = new PieChartViewModel(analyzer.NoviceStatistics);
            if (!_UserData.HiddenNoviceWish)
            {
                PieChartViewModels.Add(_NovicePieChart);
            }
        }

        private PieChartViewModel _NovicePieChart;

        private UserData _UserData;




        private ObservableCollection<PieChartViewModel> _PieChartViewModels;
        public ObservableCollection<PieChartViewModel> PieChartViewModels
        {
            get { return _PieChartViewModels; }
            set
            {
                _PieChartViewModels = value;
                OnPropertyChanged();
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(DataContext is UserData userData))
            {
                return;
            }
            var gachaList = JsonSerializer.Deserialize<List<WishData>>(File.ReadAllText(userData.WishLogFile), JsonOptions);
            var analyzer = new WishAnalyzer(gachaList);
            PieChartViewModels.Clear();
            if (analyzer.CharacterEventStatistics.Count > 0)
            {
                PieChartViewModels.Add(new PieChartViewModel(analyzer.CharacterEventStatistics));
            }
            if (analyzer.WeaponEventStatistics.Count > 0)
            {
                PieChartViewModels.Add(new PieChartViewModel(analyzer.WeaponEventStatistics));
            }
            if (analyzer.PermanentStatistics.Count > 0)
            {
                PieChartViewModels.Add(new PieChartViewModel(analyzer.PermanentStatistics));
            }
            if (analyzer.NoviceStatistics.Count > 0)
            {
                PieChartViewModels.Add(new PieChartViewModel(analyzer.NoviceStatistics));
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _UserData.HiddenNoviceWish = (bool)ToggleButton_HiddenNoviceWish.IsChecked;
            if (_UserData.HiddenNoviceWish)
            {
                if (PieChartViewModels.Contains(_NovicePieChart))
                {
                    PieChartViewModels.Remove(_NovicePieChart);
                }
            }
            else
            {
                if (!PieChartViewModels.Contains(_NovicePieChart))
                {
                    PieChartViewModels.Add(_NovicePieChart);
                }
            }
        }
    }
}
