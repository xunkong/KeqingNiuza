using KeqingNiuza.Model;
using KeqingNiuza.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeqingNiuza.View
{
    /// <summary>
    /// WishSummaryView.xaml 的交互逻辑
    /// </summary>
    public partial class WishSummaryView : UserControl
    {

        public WishSummaryViewModel ViewModel { get; set; }
        public UserData UserData { get; set; }


        public WishSummaryView()
        {
            InitializeComponent();
            UserData = MainWindowViewModel.GetSelectedUserData();
            if (UserData == null || MainWindowViewModel.WishDataList == null)
            {
                throw new NullReferenceException("没有祈愿数据");
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                await Task.Run(() => ViewModel = new WishSummaryViewModel());
                DataContext = ViewModel;
            }
        }

        private void CharacterOrderRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            var str = button.Content as string;
            ViewModel.CharacterOrder(str);
        }

        private void WeaponOrderRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            var str = button.Content as string;
            ViewModel.WeaponOrder(str);
        }

        private void Button_Detail_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button).Tag;
            ViewModel.ShowDetailView(tag as string);
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            ViewModel.ShowDetailView(grid.DataContext);
        }
    }
}
