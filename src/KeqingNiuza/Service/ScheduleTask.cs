using KeqingNiuza.Model;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeqingNiuza.Service
{
    static class ScheduleTask
    {

        private const string regepath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\原神\";

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static string GetQueryXml()
        {
            var launcherPath = Registry.GetValue(regepath, "DisplayIcon", null) as string;
            var qtPath = launcherPath.Replace("launcher.exe", "QtWebEngineProcess.exe");
            return $@"<QueryList>
  <Query Id='0' Path='Security'>
    <Select Path = 'Security' >
    *[System[band(Keywords, 9007199254740992) and
    (EventID = 4688)]] and
      *[EventData[Data[@Name = 'NewProcessName'] and
      (Data = '{qtPath}')]]
	</Select>
  </Query>
</QueryList>";
        }
        public static void AddTask(TaskTrigger trigger)
        {
            using (var t = TaskService.Instance.NewTask())
            {
                t.RegistrationInfo.Description = "刻记牛杂店定时提醒";
                if ((trigger & TaskTrigger.Logon) == TaskTrigger.Logon)
                {
                    var lt = new LogonTrigger();
                    lt.Delay = new TimeSpan(0, 1, 0);
                    t.Triggers.Add(lt);
                }
                if ((trigger & TaskTrigger.GenshinStart) == TaskTrigger.GenshinStart)
                {
                    var et = new EventTrigger();
                    et.Subscription = GetQueryXml();
                    t.Triggers.Add(et);
                }
                var exePath = Process.GetCurrentProcess().MainModule.FileName;
                var exeDir = Path.GetDirectoryName(exePath);
                t.Actions.Add(new ExecAction(exePath, "ScheduleTask", exeDir));
                TaskService.Instance.RootFolder.RegisterTaskDefinition("KeqingNiuza", t);
            }
        }

        public static void SendNotification()
        {
            if (File.Exists("UserData\\ScheduleTask.json"))
            {
                var json = File.ReadAllText("UserData\\ScheduleTask.json");
                var list = JsonSerializer.Deserialize<List<ScheduleInfo>>(json);
                foreach (var info in list)
                {
                    if (DateTime.Now > info.NextTriggerTime)
                    {
                        new ToastContentBuilder()
                        .AddText("定时提醒")
                        .AddText($"{info.Name}")
                        .AddAttributionText(info.NextTriggerTime.ToString("G"))
                        .Show();
                    }
                }
            }
        }

        public static void AddRealTimeNotifacation(IEnumerable<ScheduleInfo> list)
        {
            var notifer = ToastNotificationManagerCompat.CreateToastNotifier();
            var scheduledNofications = notifer.GetScheduledToastNotifications();
            foreach (var item in scheduledNofications)
            {
                notifer.RemoveFromSchedule(item);
            }
            foreach (var item in list)
            {
                if (item.NextTriggerTime > DateTime.Now)
                {
                    new ToastContentBuilder()
                        .AddText("定时提醒")
                        .AddText($"{item.Name}")
                        .AddAttributionText(item.NextTriggerTime.ToString("G"))
                        .Schedule(item.NextTriggerTime);
                }
            }
        }
    }

    [Flags]
    enum TaskTrigger
    {
        None = 0,
        Logon = 1,
        GenshinStart = 2,
    }
}
