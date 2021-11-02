using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Sign
{
    public class SignInInfo
    {
        /// <summary>
        /// 累积签到天数
        /// </summary>
        [JsonProperty("total_sign_day")] public int TotalSignDay { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        [JsonProperty("today")] public string Today { get; set; }
        /// <summary>
        /// 今日是否已签到
        /// </summary>
        [JsonProperty("is_sign")] public bool IsSign { get; set; }
        public bool IsNotSign => !IsSign;
        [JsonProperty("is_sub")] public bool IsSub { get; set; }
        [JsonProperty("first_bind")] public bool FirstBind { get; set; }
        [JsonProperty("month_first")] public bool MonthFirst { get; set; }
        [JsonProperty("sign_cnt_missed")] public bool SignCountMissed { get; set; }
    }
}
