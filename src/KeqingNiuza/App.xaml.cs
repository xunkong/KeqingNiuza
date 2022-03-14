using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using KeqingNiuza.View;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace KeqingNiuza
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        internal static ExtensionSetting ExtensionSetting { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Environment.CurrentDirectory = AppContext.BaseDirectory;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (e.Args.Length != 0)
            {
                if (e.Args[0] == "--schedule-task")
                {
                    await Task.Delay(2000);
                    ScheduleTask.SendNotification();
                }
                if (e.Args[0] == "--daily-check")
                {
                    await DailyCheckTask.CheckIn();
                }
                Shutdown();
            }
            else
            {
#if !DEBUG
                AppCenter.Start("", typeof(Analytics), typeof(Crashes));
                AppCenter.SetUserId(Const.UserId);
#endif
                var extensionPath = $"{Const.UserDataPath}\\ExtensionSetting.json";
                try
                {
                    if (File.Exists(extensionPath))
                    {
                        var json = File.ReadAllText(extensionPath);
                        ExtensionSetting = JsonSerializer.Deserialize<ExtensionSetting>(json);
                    }
                    if (ExtensionSetting == null)
                    {
                        ExtensionSetting = new ExtensionSetting();
                    }
                    File.WriteAllText(extensionPath, JsonSerializer.Serialize(ExtensionSetting));
                }
                catch (Exception ex)
                {
                    if (ExtensionSetting is null)
                    {
                        ExtensionSetting = new ExtensionSetting();
                    }
                    Log.OutputLog(LogType.Fault, "Load and save ExtentionSetting", ex);
                }
                MainWindow = new MainWindow();
                MainWindow.Show();
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            try
            {
                Log.OutputLog(LogType.Fault, "Crash", ex);
            }
            catch { }
            var id = Const.UserId;
            Clipboard.SetText(id.ToString());
            var msg = $"发生了未处理的错误，如有疑问请联系开发者：\n{ex.Message}\nDeviceId已复制到剪贴板：{id}";
            MessageBox.Show(msg, "KeqingNiuza");
        }
    }
}
