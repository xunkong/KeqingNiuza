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
using System.Windows.Shapes;
using KeqingNiuza.RealtimeNotes.Services;
using static KeqingNiuza.RealtimeNotes.SparsePackageUtil;

namespace KeqingNiuza.RealtimeNotes
{
    /// <summary>
    /// RegisterSparsePackageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterSparsePackageWindow : Window
    {
        public RegisterSparsePackageWindow()
        {
            InitializeComponent();
        }

        private async void Button_RegisterPackage_Click(object sender, RoutedEventArgs e)
        {
            Button_RegisterPackage.IsEnabled = false;
            if (RegisterSparsePackage(AppContext.BaseDirectory, System.IO.Path.Combine(AppContext.BaseDirectory, "KeqingNiuza.msix"), out string info))
            {
                TextBlock_State.Foreground = Brushes.Green;
                TextBlock_State.Text = "注册成功";
                await Task.Delay(500);
                try
                {
                    Process.Start(typeof(RegisterSparsePackageWindow).Assembly.Location);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    TextBlock_State.Text = $"注册成功\n重启失败：{ex.Message}";
                    LogService.Log(ex.ToString());
                }
            }
            else
            {
                TextBlock_State.Foreground = Brushes.Red;
                TextBlock_State.Text = $"注册失败：{info}";
            }
            Button_RegisterPackage.IsEnabled = true;
        }
    }
}
