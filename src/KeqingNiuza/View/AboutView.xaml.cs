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

namespace KeqingNiuza.View
{
    /// <summary>
    /// AboutView.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView : UserControl
    {

        public AboutView()
        {
            InitializeComponent();
            TextBlock_Version.Text = "版本：" + Const.Version.ToString(3);
            TextBlock_Version_All.Text = Const.Version.ToString();
        }

        public AboutView(UserData userData)
        {
            InitializeComponent();
            TextBlock_Version.Text = "版本：" + Const.Version.ToString(3);
            TextBlock_Version_All.Text = Const.Version.ToString();
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
                    var str = JsonSerializer.Serialize(result.Item3, Const.JsonOptions);
                    File.WriteAllText(result.Item2.WishLogFile, str);
                    result.Item2.LastUpdateTime = DateTime.Now;
                    Growl.Success("导入数据成功");
                }
                catch (Exception ex)
                {
                    Growl.Error(ex.Message);
                    Log.OutputLog(LogType.Error, "ImportExcelFile", ex);
                }
            }
            else
            {
                if (result.Item2 != null)
                {
                    Growl.Error("导入数据失败");
                }
            }
        }
    }
}
