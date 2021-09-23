using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;
using KeqingNiuza.Core.CloudBackup;
using KeqingNiuza.Service;

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
            InputServerUrl.Text = "https://dav.jianguoyun.com/dav/";
        }

        public CloudClient Result { get; set; }
        public Action CloseAction { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ControlCommands.Close.Execute(null, this);

        }

        public async Task<(bool isSuccessful, int code, string msg)> CloudLogin(string userName, string password, CloudType cloudType, string url = null)
        {
            var client = CloudClient.Create(userName, password, cloudType, url);
            var state = await client.ConfirmAccount();
            if (state.isSuccessful)
            {
                Result = client;
                client.SaveEncyptedAccount();
                return state;
            }
            else
            {
                return state;
            }
        }

        private async void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            var username = InputUserName.Text;
            var password = InputPassword.Password;
            var server = InputServerUrl.Text;
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
                (bool isSuccessful, int code, string msg) result = (false, 0, null);
                try
                {
                    result = await CloudLogin(username, password, CloudType.WebDav, server);
                }
                catch (Exception ex)
                {
                    InputUserName.IsEnabled = true;
                    InputPassword.IsEnabled = true;
                    Button_Login.IsEnabled = true;
                    TextBlock_Info.Text = $"登录失败: {ex.Message}";
                    TextBlock_Info.Foreground = new SolidColorBrush(Colors.Red);
                    Log.OutputLog(LogType.Warning, "BackupCloudLogin", ex);
                }
                if (result.isSuccessful)
                {
                    ControlCommands.Close.Execute(null, this);
                }
                else
                {
                    InputUserName.IsEnabled = true;
                    InputPassword.IsEnabled = true;
                    Button_Login.IsEnabled = true;
                    TextBlock_Info.Text = $"登录失败: {result.msg} ({result.code})";
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
