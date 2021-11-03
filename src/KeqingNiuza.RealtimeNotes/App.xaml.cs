using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KeqingNiuza.RealtimeNotes.Models;
using KeqingNiuza.RealtimeNotes.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Management.Deployment;
using static KeqingNiuza.RealtimeNotes.SparsePackageUtil;



namespace KeqingNiuza.RealtimeNotes
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {



        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Environment.CurrentDirectory = AppContext.BaseDirectory;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (e.Args.Any())
            {
                if (e.Args[0] == "--update-note")
                {
                    await BackgroundService.UpdateNote(e.Args[2]);
                }
                if (e.Args[0] == "--update-notes")
                {
                    await BackgroundService.UpdateNotes();
                }
                Environment.Exit(0);
            }
            else
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
                MainWindow.Activate();
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"发生了错误：\n{(e.ExceptionObject as Exception)?.Message}", "实时便笺");
            LogService.Log(e.ExceptionObject.ToString());
        }
    }
}
