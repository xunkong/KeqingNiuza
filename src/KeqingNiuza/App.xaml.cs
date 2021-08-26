using KeqingNiuza.Common;
using KeqingNiuza.Service;
using Microsoft.AppCenter;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace KeqingNiuza
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Environment.CurrentDirectory = AppContext.BaseDirectory;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (e.Args.Length != 0)
            {
                if (e.Args[0] == "Package")
                {
                    Util.ExportResourceFile();
                    Util.ExportUpdateFile();
                }
                if (e.Args[0] == "ScheduleTask")
                {
                    Task.Delay(2000);
                    ScheduleTask.SendNotification();
                }
                Shutdown();
            }
            else
            {
#if !DEBUG
                AppCenter.Start("67db8a8a-9f6e-4f36-bf69-aa61bb78245d", typeof(Analytics), typeof(Crashes));
                AppCenter.SetUserId(AppCenter.GetInstallIdAsync().Result.ToString());
#endif
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
            var id = AppCenter.GetInstallIdAsync().Result;
            Clipboard.SetText(id.ToString());
            var msg = $"发生了未处理的错误，如有疑问请联系开发者：\n{ex.Message}\nDeviceId已复制到剪贴板：{id}";
            System.Windows.MessageBox.Show(msg, "KeqingNiuza");
        }
    }
}
