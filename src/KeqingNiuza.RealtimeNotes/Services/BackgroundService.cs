using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KeqingNiuza.RealtimeNotes.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using MT = Microsoft.Win32.TaskScheduler;

namespace KeqingNiuza.RealtimeNotes.Services
{
    internal class BackgroundService
    {

        public static async Task UpdateNote(string id)
        {
            try
            {
                var bytes = File.ReadAllBytes(Const.CookiesFile);
                var cookie = Endecryption.Decrypt(bytes);
                var list = await RealtimeNotesService.GetRealtimeNotes(cookie);
                if (list.Find(x => x.Uid == id) is RealtimeNotesInfo info)
                {
                    TileService.UpdateTile(info);
                }
                new ToastContentBuilder()
                    .AddText("更新成功")
                    .AddAttributionText(DateTime.Now.ToString("G"))
                    .Show();

            }
            catch (Exception ex)
            {
                new ToastContentBuilder()
                    .AddText("更新失败")
                    .AddText(ex.Message)
                    .AddAttributionText(DateTime.Now.ToString("G"))
                    .Show();
            }
        }


        public static async Task UpdateNotes()
        {
            try
            {
                var bytes = File.ReadAllBytes(Const.CookiesFile);
                var cookie = Endecryption.Decrypt(bytes);
                var list = await RealtimeNotesService.GetRealtimeNotes(cookie);
                var ids = await TileService.FindAllAsync();
                foreach (var id in ids)
                {
                    if (list.Find(x => x.Uid == id) is RealtimeNotesInfo info)
                    {
                        TileService.UpdateTile(info);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }



        public static void AddScheduleTask()
        {
            using (var t = MT.TaskService.Instance.NewTask())
            {
                t.RegistrationInfo.Description = "刻记牛杂店-任务提醒";
                var lt = new MT.TimeTrigger();
                lt.Repetition = new MT.RepetitionPattern(TimeSpan.FromMinutes(15), TimeSpan.Zero);
                t.Triggers.Add(lt);
                var exePath = Process.GetCurrentProcess().MainModule.FileName;
                var exeDir = Path.GetDirectoryName(exePath);
                t.Actions.Add(new MT.ExecAction(exePath, "--update-notes"));
                MT.TaskService.Instance.RootFolder.RegisterTaskDefinition("实时便笺定时刷新任务", t);
            }
        }

    }
}
