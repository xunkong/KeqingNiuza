using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text.Json;
using KeqingNiuza.Model;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using static KeqingNiuza.Service.Const;

namespace KeqingNiuza.Service
{
    static class ScheduleTask
    {

        private const string regepath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\原神\";
        private const string regepath_sea = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\原神\";

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static string GetQueryXml()
        {
            var launcherPath = Registry.GetValue(regepath, "DisplayIcon", null) as string;
            if (launcherPath == null)
            {
                launcherPath = Registry.GetValue(regepath_sea, "DisplayIcon", null) as string;
            }
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
                t.RegistrationInfo.Description = "刻记牛杂店-任务提醒";
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
                t.Actions.Add(new ExecAction(exePath, "--schedule-task"));
                TaskService.Instance.RootFolder.RegisterTaskDefinition("KeqingNiuza ScheduleTask", t);
            }
        }

        public static void SendNotification()
        {
            if (File.Exists($"{UserDataPath}\\ScheduleTask.json"))
            {
                var json = File.ReadAllText($"{UserDataPath}\\ScheduleTask.json");
                var list = JsonSerializer.Deserialize<List<ScheduleInfo>>(json);
                foreach (var info in list)
                {
                    if (info.TriggerType == ScheduleInfoTriggerType.None)
                    {
                        continue;
                    }
                    if (info.TriggerType == ScheduleInfoTriggerType.Recovery)
                    {
                        if (DateTime.Now > info.NextMaxValueTime && info.IsEnable)
                        {
                            new ToastContentBuilder()
                            .AddText("任务提醒")
                            .AddText($"{info.Name} {info.MaxValue}/{info.MaxValue}")
                            .AddAttributionText(info.NextMaxValueTime.ToString("G"))
                            .Show();
                        }
                    }
                    else
                    {
                        if (DateTime.Now > info.NextTriggerTime && info.IsEnable)
                        {
                            new ToastContentBuilder()
                            .AddText("任务提醒")
                            .AddText($"{info.Name}")
                            .AddAttributionText(info.NextTriggerTime.ToString("G"))
                            .Show();
                        }
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
            foreach (var info in list)
            {
                if (info.TriggerType == ScheduleInfoTriggerType.None)
                {
                    continue;
                }
                if (info.TriggerType == ScheduleInfoTriggerType.Recovery)
                {
                    if (info.NextMaxValueTime > DateTime.Now && info.IsEnable)
                    {
                        new ToastContentBuilder()
                        .AddText("任务提醒")
                        .AddText($"{info.Name} {info.MaxValue}/{info.MaxValue}")
                        .AddAttributionText(info.NextMaxValueTime.ToString("G"))
                        .Schedule(info.NextMaxValueTime);
                    }
                }
                else
                {
                    if (info.NextTriggerTime > DateTime.Now && info.IsEnable)
                    {
                        new ToastContentBuilder()
                        .AddText("任务提醒")
                        .AddText($"{info.Name}")
                        .AddAttributionText(info.NextTriggerTime.ToString("G"))
                        .Schedule(info.NextTriggerTime);
                    }
                }
            }
        }


        public static void TestNotifacation()
        {
            new ToastContentBuilder()
                .AddText("通知测试")
                .AddText($"这是一条测试通知")
                .AddAttributionText(DateTime.Now.ToString("G"))
                .Show();
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
