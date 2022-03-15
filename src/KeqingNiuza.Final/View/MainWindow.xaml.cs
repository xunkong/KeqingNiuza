using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using KeqingNiuza.ViewModel;
using static KeqingNiuza.Service.Const;

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


        private void Window_Main_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowState = Properties.Settings.Default.IsWindowMaximized ? WindowState.Maximized : WindowState.Normal;
            }
            catch (Exception ex)
            {
                WindowState = WindowState.Normal;
                Log.OutputLog(LogType.Warning, "Window_Main_Loaded", ex);
            }
            // 如果软件窗口超出屏幕边界，则最大化
            if (Native.IsWindowBeyondBounds(ActualWidth, ActualHeight))
            {
                WindowState = WindowState.Maximized;
            }
            InitSideMenuChecked();
        }

        private void InitSideMenuChecked()
        {
            if (ViewModel.ViewContent is WishSummaryView)
            {
                SideMenu_WishSummaryView.IsChecked = true;
            }
        }

        private void Window_Main_Closed(object sender, EventArgs e)
        {
            ViewModel.SaveConfig();
            Properties.Settings.Default.IsWindowMaximized = WindowState == WindowState.Maximized;
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
                var client = await Dialog.Show(new CloudLoginDialog()).Initialize<CloudLoginDialog>(x => { }).GetResultAsync<KeqingNiuza.Core.CloudBackup.CloudClient>();
                if (client != null)
                {
                    ViewModel.CloudClient = client;
                    Popup_Cloud.IsOpen = true;
                    Growl.Success("登录成功");
                }
            }
            else
            {
                Popup_Cloud.IsOpen = true;
            }
        }

        private void Button_Logout_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists($"{UserDataPath}\\Account"))
            {
                File.Delete($"{UserDataPath}\\Account");
            }
            ViewModel.CloudClient = null;
            Popup_Cloud.IsOpen = false;
            Growl.Success("已删除保存的账号和密码");
        }


        private async void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            Button_Load.IsEnabled = false;
            Button_LoadAll.IsEnabled = false;
            await ViewModel.UpdateWishData();
            Button_LoadAll.IsEnabled = true;
            Button_Load.IsEnabled = true;
        }


        private async void Button_LoadAll_Click(object sender, RoutedEventArgs e)
        {
            Button_Load.IsEnabled = false;
            Button_LoadAll.IsEnabled = false;
            await ViewModel.UpdateWishData(true);
            Button_LoadAll.IsEnabled = true;
            Button_Load.IsEnabled = true;
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

        private async void Button_Restore_Click(object sender, RoutedEventArgs e)
        {
            Button_Restore.IsEnabled = false;
            await ViewModel.CloudRestoreFileArchive();
            Button_Restore.IsEnabled = true;
        }

        private void RadioButton_SideMenu_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            var tag = radioButton.Tag as string;
            ViewModel.ChangeViewContent(tag);
        }


        private async void Ellipse_Avatar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Popup_Uid.IsOpen = false;
            await ViewModel.ChangeAvatar();
        }

        private void Button_ChangeUid_Click(object sender, RoutedEventArgs e)
        {
            Grid_UserData.Visibility = Visibility.Visible;
        }

        private void Window_Main_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    BorderThickness = new Thickness(0);
                    break;
                case WindowState.Minimized:
                    BorderThickness = new Thickness(0);
                    break;
                case WindowState.Maximized:
                    BorderThickness = new Thickness(7);
                    break;
                default:
                    break;
            }
        }

        private void ListView_UserData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListView_UserData.SelectedItem == null)
            {
                return;
            }
            ViewModel.ChangeUid(ListView_UserData.SelectedItem);
            Popup_Uid.IsOpen = false;
        }

        private void Popup_Uid_Closed(object sender, EventArgs e)
        {
            Grid_UserData.Visibility = Visibility.Collapsed;
            ViewModel.LoadWishDataProgress = null;
        }

        private void Button_AddUid_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddNewUid();
            Grid_UserData.Visibility = Visibility.Collapsed;
        }

        private async void Button_DeleteUid_Click(object sender, RoutedEventArgs e)
        {
            var userData = (sender as Button).DataContext as UserData;
            if (userData == null)
            {
                return;
            }
            Popup_Uid.IsOpen = false;
            var result = await Dialog.Show(new DeleteUidDialog(userData.Uid)).Initialize<DeleteUidDialog>(x => { }).GetResultAsync<bool>();
            if (result)
            {
                ViewModel.UserDataList.Remove(userData);
                if (ViewModel.SelectedUserData == userData)
                {
                    if (ViewModel.UserDataList.Any())
                    {
                        ViewModel.SelectedUserData = ViewModel.UserDataList.First();
                    }
                    else
                    {
                        ViewModel.SelectedUserData = null;
                        if (File.Exists($"{UserDataPath}\\Config.json"))
                        {
                            File.Delete($"{UserDataPath}\\Config.json");
                        }
                    }
                }
                ViewModel.SaveConfig();
                if (File.Exists(userData.WishLogFile))
                {
                    File.Delete(userData.WishLogFile);
                }
                ViewModel.ReloadViewContent();
                await Task.Delay(1000);
                Growl.Success($"Uid:{userData.Uid}已删除");
            }
        }

        private void SideMenu_Manual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@".\使用手册.pdf");
            }
            catch (Exception ex)
            {
                Log.OutputLog(LogType.Warning, "Open manual", ex);
                Growl.Warning(ex.Message);
            }
        }


    }
}
