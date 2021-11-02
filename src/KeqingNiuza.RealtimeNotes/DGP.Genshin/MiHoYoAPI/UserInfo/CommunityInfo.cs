using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class CommunityInfo
    {
        [JsonProperty("is_realname")] public bool IsRealname { get; set; }
        [JsonProperty("agree_status")] public bool AgreeStatus { get; set; }
        [JsonProperty("silent_end_time")] public long SilentEndTime { get; set; }
        [JsonProperty("forbid_end_time")] public long ForbidEndTime { get; set; }
        [JsonProperty("info_upd_time")] public long InfoUpdTime { get; set; }
        [JsonProperty("privacy_invisible")] public PrivacyInvisible PrivacyInvisible { get; set; }
        [JsonProperty("notify_disable")] public NotifyDisable NotifyDisable { get; set; }
        [JsonProperty("has_initialized")] public bool HasInitialized { get; set; }
        [JsonProperty("user_func_status")] public UserFuncStatus UserFuncStatus { get; set; }
        [JsonProperty("forum_silent_info")] public List<string> ForumSilentInfo { get; set; }
    }
}