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
using KeqingNiuza.Core.Wish;
using KeqingNiuza.Model;
using KeqingNiuza.Service;

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
            TextBlock_Version.Text = "版本：" + Service.Const.FileVersion;
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }

        private async void Button_ImportExcel_Click(object sender, RoutedEventArgs e)
        {
            if (await Dialog.Show(new ExcelImportDialog()).GetResultAsync<bool>())
            {
                Growl.Success("导入数据成功");
                (Application.Current.MainWindow as MainWindow).ViewModel.ChangeViewContent("WishSummaryView");
            }
        }

        private async void Button_InputUrl_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            await mainWindow.ViewModel.UpdateWishData(TextBox_InputUrl.Text);
        }
    }
}
