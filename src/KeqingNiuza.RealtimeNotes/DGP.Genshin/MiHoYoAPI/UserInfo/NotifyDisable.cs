using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class NotifyDisable
    {
        [JsonProperty("reply")] public bool Reply { get; set; }
        [JsonProperty("upvote")] public bool Upvote { get; set; }
        [JsonProperty("follow")] public bool Follow { get; set; }
        [JsonProperty("system")] public bool System { get; set; }
        [JsonProperty("chat")] public bool Chat { get; set; }
    }
}
