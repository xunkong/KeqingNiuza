using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 层
    /// </summary>
    public class Floor
    {
        [JsonProperty("index")] public int Index { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
        [JsonProperty("is_unlock")] public string IsUnlock { get; set; }
        [JsonProperty("settle_time")] public string SettleTime { get; set; }
        [JsonProperty("star")] public int Star { get; set; }
        [JsonProperty("max_star")] public int MaxStar { get; set; }
        [JsonProperty("levels")] public List<Level> Levels { get; set; }
    }
}
