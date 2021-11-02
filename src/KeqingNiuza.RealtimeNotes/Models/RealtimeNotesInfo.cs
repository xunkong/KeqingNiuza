using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGP.Genshin.MiHoYoAPI.Record.DailyNote;
using DGP.Genshin.MiHoYoAPI.User;

namespace KeqingNiuza.RealtimeNotes.Models
{
    public class RealtimeNotesInfo
    {

        public string Uid { get; set; }


        public string NickName { get; set; }


        public int Level { get; set; }


        public string Region { get; set; }

        /// <summary>
        /// 便笺信息最后更新的时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 当前树脂
        /// </summary>
        public int CurrentResin { get; set; }

        /// <summary>
        /// 最大树脂
        /// </summary>
        public int MaxResin { get; set; }

        /// <summary>
        /// 树脂恢复时间，按秒计算
        /// </summary>
        public int ResinRecoveryTime { get; set; }


        public string ResinRecoveryTimeFormatted => TimeSpan.FromSeconds(ResinRecoveryTime).ToString(@"hh\:mm");

        /// <summary>
        /// 已完成的委托数
        /// </summary>
        public int FinishedTaskNum { get; set; }

        /// <summary>
        /// 总委托数
        /// </summary>
        public int TotalTaskNum { get; set; }

        /// <summary>
        /// 委托奖励是否领取
        /// </summary>
        public bool IsExtraTaskRewardReceived { get; set; }


        /// <summary>
        /// 剩余周本折扣数
        /// </summary>
        public int RemainResinDiscountNum { get; set; }

        /// <summary>
        /// 周本折扣总次数
        /// </summary>
        public int ResinDiscountNumLimit { get; set; }

        /// <summary>
        /// 当前派遣数
        /// </summary>
        public int CurrentExpeditionNum { get; set; }

        /// <summary>
        /// 最大派遣数
        /// </summary>
        public int MaxExpeditionNum { get; set; }

        /// <summary>
        /// 派遣详细信息
        /// </summary>
        public List<Expedition> Expeditions { get; set; }


        /// <summary>
        /// 是否固定于开始屏幕
        /// </summary>
        public bool IsPinned { get; set; }



        public static RealtimeNotesInfo FromGameRole(UserGameRole role)
        {
            var note = new RealtimeNotesInfo();
            note.Uid = role.GameUid;
            note.NickName = role.Nickname;
            note.Level = role.Level;
            note.Region = role.RegionName;
            return note;
        }


        public void UpdateNoteInfo(DailyNote note)
        {
            LastUpdateTime = DateTime.Now;
            CurrentResin = note.CurrentResin;
            MaxResin = note.MaxResin;
            ResinRecoveryTime = int.Parse(note.ResinRecoveryTime);
            FinishedTaskNum = note.FinishedTaskNum;
            TotalTaskNum = note.TotalTaskNum;
            IsExtraTaskRewardReceived = note.IsExtraTaskRewardReceived;
            RemainResinDiscountNum = note.RemainResinDiscountNum;
            ResinDiscountNumLimit = note.ResinDiscountNumLimit;
            CurrentExpeditionNum = note.CurrentExpeditionNum;
            MaxExpeditionNum = note.MaxExpeditionNum;
            Expeditions = note.Expeditions;
        }


    }
}
