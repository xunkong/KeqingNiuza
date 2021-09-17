using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using KeqingNiuza.CloudBackup;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32.TaskScheduler;
using static KeqingNiuza.Service.Const;

namespace KeqingNiuza.Service
{
    public static class DailyCheckTask
    {
        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void AddTask(bool isEnable, DateTime starTime, TimeSpan randomDelay)
        {
            using (var t = TaskService.Instance.NewTask())
            {
                t.RegistrationInfo.Description = "刻记牛杂店-米游社签到";
                if (isEnable)
                {
                    t.Triggers.Add(new DailyTrigger
                    {
                        DaysInterval = 1,
                        Enabled = true,
                        StartBoundary = starTime,
                        RandomDelay = randomDelay
                    });
                }
                t.Settings.StartWhenAvailable = true;
                var exePath = Process.GetCurrentProcess().MainModule.FileName;
                t.Actions.Add(new ExecAction(exePath, "--daily-check"));
                TaskService.Instance.RootFolder.RegisterTaskDefinition("KeqingNiuza DailyCheck", t);
            }
        }


        public static async System.Threading.Tasks.Task CheckIn()
        {
            string checkLog = null;
            GenshinDailyHelper.Program.PrintLog = (log) => checkLog += $"[{DateTime.Now:HH:mm:ss}]{log}";
            if (!File.Exists($@"{UserDataPath}\DailyCheckCookies"))
            {
                return;
            }
            try
            {
                var bytes = File.ReadAllBytes($@"{UserDataPath}\DailyCheckCookies");
                var cookies = Endecryption.Decrypt(bytes);
                await GenshinDailyHelper.Program.Checkin(new string[] { cookies });
                if (Properties.Settings.Default.DialyCheck_AlwaysShowResult)
                {
                    new ToastContentBuilder().AddText("签到已完成").Show();
                }
                var log = $"[{DateTime.Now}]\n{checkLog}\n\n";
                Directory.CreateDirectory(LogPath);
                File.AppendAllText($@"{LogPath}\DailyCheckLog.txt", log);
            }
            catch (Exception ex)
            {
                new ToastContentBuilder().AddText("签到失败").AddText(ex.Message).Show();
                Log.OutputLog(LogType.Warning, "DailyCheckIn", ex);
                var errorLog = $"[{DateTime.Now}]\n{checkLog}\n\n";
                Directory.CreateDirectory(LogPath);
                File.AppendAllText($@"{LogPath}\DailyCheck_ErrorLog.txt", errorLog);
            }
        }
    }
}
