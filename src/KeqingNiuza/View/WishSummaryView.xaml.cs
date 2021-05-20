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
            if (UserData == null)
            {
                throw new NullReferenceException("没有数据");
            }
            ViewModel = new WishSummaryViewModel(UserData);
            DataContext = ViewModel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //todo LiveCharts 图标初始化只能在UI线程调用
            //if (ViewModel == null)
            //{
            //    await Task.Run(() => ViewModel = new WishSummaryViewModel(UserData));
            //    DataContext = ViewModel;
            //}
        }
    }
}
