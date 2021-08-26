using KeqingNiuza.Wish;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeqingNiuza.View
{
    /// <summary>
    /// WishPredictionView.xaml 的交互逻辑
    /// </summary>
    public partial class WishPredictionView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public WishPredictionView()
        {
            InitializeComponent();
        }


        private ChartValues<double> _Value;
        public ChartValues<double> Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged();
            }
        }

        private int _XSection;
        public int XSection
        {
            get { return _XSection; }
            set
            {
                _XSection = value;
                OnPropertyChanged();
            }
        }

        private double _YSection;
        public double YSection
        {
            get { return _YSection; }
            set
            {
                _YSection = value;
                OnPropertyChanged();
            }
        }

        private Visibility _AxisSectionVisibility = Visibility.Hidden;
        public Visibility AxisSectionVisibility
        {
            get { return _AxisSectionVisibility; }
            set
            {
                _AxisSectionVisibility = value;
                OnPropertyChanged();
            }
        }


        private bool _ShowSectionDataLabel;
        public bool ShowSectionDataLabel
        {
            get { return _ShowSectionDataLabel; }
            set
            {
                _ShowSectionDataLabel = value;
                OnPropertyChanged();
            }
        }



        public Func<double, string> YSectionFormatter => value => value.ToString("P4");

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var count = (int)NumericUpDown.Value;
            var index = ComboBox.SelectedIndex;
            await Task.Run(() =>
            {
                switch (index)
                {
                    case 0:
                        Value = new ChartValues<double>(Prediction.GetCharacterDensityAndDistributionWithUp(count).distribution.Prepend(0));
                        break;
                    case 1:
                        Value = new ChartValues<double>(Prediction.GetCharacterDensityAndDistribution(count).distribution.Prepend(0));
                        break;
                    case 2:
                        Value = new ChartValues<double>(Prediction.GetSpecifiedWeaponDensityAndDistribution(count).distribution.Prepend(0));
                        break;
                    case 3:
                        Value = new ChartValues<double>(Prediction.GetWeaponDensityAndDistribution(count).distribution.Prepend(0));
                        break;
                }
            });
        }


        private void CartesianChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (Value != null)
            {
                var chart = sender as CartesianChart;
                var point = e.GetPosition(chart);
                var p = chart.ConvertToChartValues(point);
                XSection = (int)Math.Round(p.X);
                try
                {
                    YSection = Value[XSection];
                }
                catch { }
            }
        }

        private void CartesianChart_MouseEnter(object sender, MouseEventArgs e)
        {
            AxisSectionVisibility = Visibility.Visible;
            ShowSectionDataLabel = true;
        }

        private void CartesianChart_MouseLeave(object sender, MouseEventArgs e)
        {
            AxisSectionVisibility = Visibility.Hidden;
            ShowSectionDataLabel = false;
        }


    }
}
