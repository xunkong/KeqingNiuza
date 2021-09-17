using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using KeqingNiuza.Wish;

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
            var name = typeof(MainWindow).Assembly.Location;
            var v = FileVersionInfo.GetVersionInfo(name);
            TextBlock_Version.Text = "版本：" + v.FileVersion;
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }

        private async void Button_ImportExcel_Click(object sender, RoutedEventArgs e)
        {
            var result = await Dialog.Show(new ExcelImportDialog()).GetResultAsync<(bool, UserData, List<WishData>)>();
            if (result.Item1)
            {
                try
                {
                    var str = JsonSerializer.Serialize(result.Item3, Service.Const.JsonOptions);
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

        private async void Button_InputUrl_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            await mainWindow.ViewModel.UpdateWishData(TextBox_InputUrl.Text);
        }
    }
}
