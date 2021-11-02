using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Sign
{
    public class SignInReward
    {
        /// <summary>
        /// 月份
        /// </summary>
        [JsonProperty("month")] public string Month { get; set; }
        [JsonProperty("awards")] public List<SignInAward> Awards { get; set; }
    }
}
