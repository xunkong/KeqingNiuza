using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using Windows.Storage;

namespace KeqingNiuza.RealtimeNotes
{
    /// <summary>
    /// SetCookieWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetCookieWindow : Window
    {
        public SetCookieWindow()
        {
            InitializeComponent();
            Loaded += SetCookieWindow_Loaded;
        }


        private void SetCookieWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Const.CookiesFile))
            {
                try
                {
                    var bytes = File.ReadAllBytes(Const.CookiesFile);
                    TextBox_Cookie.Text = Endecryption.Decrypt(bytes);
                }
                catch (Exception ex)
                {
                    TextBlock_StateText.Text = $"无法读取已保存的Cookie：{ex.Message}";
                    LogService.Log(ex.ToString());

                }
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_Cookie.Text))
            {
                TextBlock_StateText.Text = "没有输入";
                return;
            }
            try
            {
                var bytes = Endecryption.Encrypt(TextBox_Cookie.Text);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Const.CookiesFile));
                File.WriteAllBytes(Const.CookiesFile, bytes);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                TextBlock_StateText.Text = $"保存时出现错误：{ex.Message}";
                LogService.Log(ex.ToString());
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
