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
using System.Windows.Shapes;
using KeqingNiuza.ViewModel;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControl.Data;
using GenshinHelper.Desktop.View;

namespace KeqingNiuza.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;

        }

        private async void Window_Main_Loaded(object sender, RoutedEventArgs e)
        {
            InitSideMenuChecked();
#if !DEBUG
            var result = await ViewModel.TestUpdate();
            if (result)
            {
                Closed += ViewModel.CallUpdate;
            }
#endif
        }

        private void InitSideMenuChecked()
        {
            if (ViewModel.ViewContent is GachaAnalysisView)
            {
                SideMenu_WishSummary.IsChecked = true;
            }
        }

        private void Window_Main_Closed(object sender, EventArgs e)
        {
#warning 重构UI后记得删除
            ViewModel.SaveConfig();
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Maxmize_Click(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private void Button_Uid_Click(object sender, RoutedEventArgs e)
        {
            Popup_Uid.IsOpen = true;
        }

        private async void Button_Cloud_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CloudClient == null)
            {
                var client = await Dialog.Show(new CloudLoginDialog()).Initialize<CloudLoginDialog>(x => { }).GetResultAsync<CloudBackup.CloudClient>();
                if (client != null)
                {
                    ViewModel.CloudClient = client;
                    Popup_Cloud.IsOpen = true;
                    Growl.Success(new GrowlInfo { Message = "登录成功", });
                }
            }
            else
            {
                Popup_Cloud.IsOpen = true;
            }
        }

        private async void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            Button_Load.IsEnabled = false;
            await ViewModel.LoadDataFromGenshinLogFile();
            Button_Load.IsEnabled = true;
        }

        private async void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            Button_Update.IsEnabled = false;
            await ViewModel.UpdateDataFromUrl();
            Button_Update.IsEnabled = true;
        }

        private void Button_Export_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExportExcelFile();
        }


        private async void Button_Backup_Click(object sender, RoutedEventArgs e)
        {
            Button_Backup.IsEnabled = false;
            await ViewModel.CloudBackupFileArchive();
            Button_Backup.IsEnabled = true;
        }

        private void RadioButton_SideMenu_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            ViewModel.SideMenuChangeContent(radioButton.Name);
        }


        private async void Ellipse_Avatar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Popup_Uid.IsOpen = false;
            await ViewModel.ChangeAvatar();
        }

        private async void Button_ChangeUid_Click(object sender, RoutedEventArgs e)
        {
            Popup_Uid.IsOpen = false;
            await ViewModel.ChangeUid();
        }
    }
}
