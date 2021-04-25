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
using HandyControl.Interactivity;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using KeqingNiuza.CloudBackup;
using HandyControl.Controls;

namespace KeqingNiuza.View
{
    /// <summary>
    /// CloudLoginDialog.xaml 的交互逻辑
    /// </summary>
    public partial class CloudLoginDialog : UserControl, IDialogResultable<CloudClient>
    {
        public CloudLoginDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        public CloudClient Result { get; set; }
        public Action CloseAction { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ControlCommands.Close.Execute(null, this);

        }

        public async Task<bool> CloudLogin(string userName, string password, CloudType cloudType)
        {
            var client = CloudClient.Create(userName, password, cloudType);
            var state = await client.ConfirmAccount();
            if (state)
            {
                Result = client;
                client.SaveEncyptedAccount();
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            var username = InputUserName.Text;
            var password = InputPassword.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TextBlock_Info.Text = "用户名或密码为空";
                TextBlock_Info.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            else
            {
                InputUserName.IsEnabled = false;
                InputPassword.IsEnabled = false;
                Button_Login.IsEnabled = false;
                TextBlock_Info.Text = "正在登录";
                TextBlock_Info.Foreground = new SolidColorBrush(Colors.Gray);
                var result = await CloudLogin(username, password, CloudType.Jianguoyun);
                if (result)
                {
                    ControlCommands.Close.Execute(null, this);
                }
                else
                {
                    InputUserName.IsEnabled = true;
                    InputPassword.IsEnabled = true;
                    Button_Login.IsEnabled = true;
                    TextBlock_Info.Text = "登录失败";
                    TextBlock_Info.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            ControlCommands.Close.Execute(null, this);
        }
    }
}
