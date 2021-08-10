using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using HandyControl.Controls;
using KeqingNiuza.Common;
using KeqingNiuza.Service;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Uwp.Notifications;

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
#endif
            }

        }

    }
}
