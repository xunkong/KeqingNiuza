using KeqingNiuza.Model;
using KeqingNiuza.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeqingNiuza.View
{
    /// <summary>
    /// WishAchievementView.xaml 的交互逻辑
    /// </summary>
    public partial class WishAchievementView : UserControl
    {
        public WishAchievementView()
        {
            InitializeComponent();
            UserData = MainWindowViewModel.GetSelectedUserData();
            if (UserData == null || MainWindowViewModel.WishDataList == null)
            {
                throw new NullReferenceException("没有祈愿数据");
            }
        }

        public WishAchievementViewModel ViewModel { get; set; }

        public UserData UserData { get; set; }

        private async void UserControl_WishAchievement_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                await Task.Run(() => ViewModel = new WishAchievementViewModel());
                DataContext = ViewModel;
            }

        }
    }
}
