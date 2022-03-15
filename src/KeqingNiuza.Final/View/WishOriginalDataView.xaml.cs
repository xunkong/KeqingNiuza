using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using KeqingNiuza.Model;
using KeqingNiuza.ViewModel;

namespace KeqingNiuza.View
{
    /// <summary>
    /// WishOriginalDataView.xaml 的交互逻辑
    /// </summary>
    public partial class WishOriginalDataView : UserControl
    {

        public WishOriginalDataViewModel ViewModel { get; set; }
        public UserData UserData { get; set; }


        public WishOriginalDataView()
        {
            InitializeComponent();
            UserData = MainWindowViewModel.GetSelectedUserData();
            if (UserData == null || MainWindowViewModel.WishDataList == null)
            {
                throw new NullReferenceException("没有祈愿数据");
            }
        }


        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void Buton_Reset_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetFilter();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                await Task.Run(() => ViewModel = new WishOriginalDataViewModel());
                DataContext = ViewModel;
            }
        }
    }
}
