using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.Avatar
{
    /// <summary>
    /// 圣遗物套装效果
    /// </summary>
    public class ReliquaryAffix
    {
        [JsonProperty("activation_number")] public int ActivationNumber { get; set; }
        [JsonProperty("effect")] public string Effect { get; set; }
    }
}
