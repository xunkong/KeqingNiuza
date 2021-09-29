using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace KeqingNiuza.Launcher
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Environment.CurrentDirectory = AppContext.BaseDirectory;
            var ps = Process.GetCurrentProcess();
            var ps_Launcher = Process.GetProcessesByName("KeqingNiuza Launcher").ToList();
            var ps_KeqingNiuza = Process.GetProcessesByName("KeqingNiuza").ToList();
            var p_removing = ps_Launcher.First(x => x.Id == ps.Id);
            if (Directory.Exists(".\\deleting"))
            {
                Directory.Delete(".\\deleting", true);
            }
            if (ps_Launcher != null)
            {
                ps_Launcher.Remove(p_removing);
            }
            if (!e.Args.Any())
            {
                if (!File.Exists(".\\UserData\\allow_appcenter"))
                {
                    var result = MessageBox.Show("继续使用则代表同意主程序在运行过程中使用 MS App Center 收集非个人信息用于改善体验（主要是修Bug）。", "KeqingNiuza Launcher", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        Directory.CreateDirectory(".\\UserData");
                        File.Create(".\\UserData\\allow_appcenter").Dispose();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
                // 无命令行参数
                if (ps_Launcher.Any())
                {
                    // 若已存在 KeqingNiuza Launcher.exe 进程，不再启动新进程
                    Environment.Exit(0);
                }
                if (ps_KeqingNiuza.Any())
                {
                    // 若已存在 KeqingNiuza.exe 进程，则切换至其窗口
                    var p = ps_KeqingNiuza.First();
                    Util.SwitchToThisWindow(p.MainWindowHandle, true);
                    Environment.Exit(0);
                }
                else
                {
                    // 启动新进程
                    MainWindow = new MainWindow();
                    MainWindow.Show();
                    return;
                }
            }
            else
            {
                // 杀死KeqingNiuza.exe并更新
                if (e.Args[0] == "--update")
                {
                    foreach (var p in ps_KeqingNiuza)
                    {
                        p.Kill();
                    }
                    MainWindow = new MainWindow();
                    MainWindow.Show();
                    return;
                }

                // 下载推荐壁纸
                if (e.Args[0] == "--download-wallpaper")
                {
                    MainWindow = new MainWindow("--download-wallpaper");
                    MainWindow.Show();
                    return;
                }

                // 删除自己
                // 此命令一定要带另外两个参数 path:待删除的文件夹 processId:调用进程的Id
                if (e.Args[0] == "--delete-self")
                {
                    Util.CountdownToShutdown();
                    var path = e.Args[1];
                    var processId = int.Parse(e.Args[2]);
                    await Util.DeleteSelf(path, processId);
                    return;
                }


                if (e.Args[0] == "--deploy")
                {
                    Util.Deploy();
                    Environment.Exit(0);
                }

                if (e.Args[0] == "--deploy-wallpaper")
                {
                    Util.DeployWallpaper();
                    Environment.Exit(0);
                }

                Environment.Exit(0);

            }
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            try
            {
                Util.OutputLog(ex);
            }
            catch { }
            MessageBox.Show(ex.Message + "\n详细信息已保存在日志中。", "KeqingNiuza Launcher");
            Environment.Exit(1);
        }


    }
}
