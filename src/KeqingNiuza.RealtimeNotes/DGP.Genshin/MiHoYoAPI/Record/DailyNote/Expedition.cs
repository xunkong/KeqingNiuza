using Newtonsoft.Json;
using System;

namespace DGP.Genshin.MiHoYoAPI.Record.DailyNote
{
    /// <summary>
    /// 探索派遣
    /// </summary>
    public class Expedition
    {
        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("avatar_side_icon")] public string AvatarSideIcon { get; set; }
        /// <summary>
        /// 状态 Ongoing:派遣中 Finished:已完成
        /// </summary>
        [JsonProperty("status")] public string Status { get; set; }

        /// <summary>
        /// 派遣是否完成
        /// </summary>
        public bool IsFinished => Status.ToLower() == "finished";

        /// <summary>
        /// 剩余时间
        /// </summary>
        [JsonProperty("remained_time")] public string RemainedTime { get; set; }

        public string RemainedTimeFormatted
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RemainedTime))
                {
                    return null;
                }
                TimeSpan ts = new TimeSpan(0, 0, int.Parse(RemainedTime));
                return ts.ToString(@"hh\:mm");
            }
        }
    }
}
