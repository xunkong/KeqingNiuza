using KeqingNiuza.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using KeqingNiuza.Wish;
using System.Text.Json;
using KeqingNiuza.Model;
using System.IO;
using KeqingNiuza.Service;
using Const = KeqingNiuza.Common.Const;
using System.Net.Http;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KeqingNiuza.View
{
    /// <summary>
    /// AboutView.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AboutView()
        {
            InitializeComponent();
            TextBlock_Version.Text = "版本：" + Const.Version.ToString(3);
            TextBlock_Version_All.Text = Const.Version.ToString();
        }

        public bool IsAutoUpdate
        {
            get => Properties.Settings.Default.IsAutoUpdate;
            set
            {
                Properties.Settings.Default.IsAutoUpdate = value;
                OnPropertyChanged();
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }

        private async void Button_TestUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button_TestUpdate.IsEnabled = false;
            LoadingCircle_TestUpdate.Visibility = Visibility.Visible;
            try
            {
                var updater = new Updater();
                var info = await updater.GetUpdateInfo();
                if (info)
                {
                    await updater.PrepareUpdateFiles();
                    updater.CallUpdateWhenExit();
                    Log.OutputLog(LogType.Info, "Update files prepare finished");
                    Growl.Success("更新文件准备完毕，关闭窗口开始更新");
                }
                else
                {
                    Growl.Info("已是最新版本");
                }
            }
            catch (Exception ex)
            {
                Growl.Warning(ex.Message);
                Log.OutputLog(LogType.Warning, "TestUpdate", ex);
            }

            LoadingCircle_TestUpdate.Visibility = Visibility.Hidden;
            Button_TestUpdate.IsEnabled = true;
        }


        private async void Button_TestResource_Click(object sender, RoutedEventArgs e)
        {
            Button_TestResource.IsEnabled = false;
            LoadingCircle_TestResource.Visibility = Visibility.Visible;
            try
            {
                var updater = new Updater();
                var info = await updater.UpdateResourceFiles();
                if (info)
                {
                    updater.CallUpdateWhenExit();
                    Growl.Success("关闭窗口完成资源文件的替换");
                }
                else
                {
                    Growl.Info("资源文件已是最新");
                }
            }
            catch (Exception ex)
            {
                Growl.Warning(ex.Message);
                Log.OutputLog(LogType.Warning, "TestResource", ex);
            }

            LoadingCircle_TestResource.Visibility = Visibility.Hidden;
            Button_TestResource.IsEnabled = true;
        }

        private async void Button_ImportExcel_Click(object sender, RoutedEventArgs e)
        {
            var result = await Dialog.Show(new ExcelImportDialog()).GetResultAsync<(bool, UserData, List<WishData>)>();
            if (result.Item1)
            {
                try
                {
                    var str = JsonSerializer.Serialize(result.Item3, Const.JsonOptions);
                    File.WriteAllText(result.Item2.WishLogFile, str);
                    result.Item2.LastUpdateTime = DateTime.Now;
                    Growl.Success("导入数据成功");
                }
                catch (Exception ex)
                {
                    Growl.Warning(ex.Message);
                    Log.OutputLog(LogType.Warning, "ImportExcelFile", ex);
                }
            }
            else
            {
                if (result.Item2 != null)
                {
                    Growl.Warning("导入数据失败");
                }
            }
        }


        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            const string path = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/";
            const string _UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36 Edg/89.0.774.68";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
            try
            {
#if TestCDN

                TextBlock_UpdateContent.Text = await client.GetStringAsync(path + "UpdateContent_Debug.txt");
                TextBlock_News.Text = await client.GetStringAsync(path + "News_Debug.txt");

#else

                TextBlock_UpdateContent.Text = await client.GetStringAsync(path + "UpdateContent.txt");
                TextBlock_News.Text = await client.GetStringAsync(path + "News.txt");

#endif

                if (!string.IsNullOrWhiteSpace(TextBlock_UpdateContent.Text))
                {
                    TextBlock_UpdateContent.Visibility = Visibility.Visible;
                }
                if (!string.IsNullOrWhiteSpace(TextBlock_News.Text))
                {
                    StackPanel_News.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Growl.Warning(ex.Message);
            }
            LoadingCircle_UpdateContent.Visibility = Visibility.Collapsed;
        }

        private async void Button_ImputUrl_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            await mainWindow.ViewModel.UpdateWishData(TextBox_InputUrl.Text);
        }
    }
}
