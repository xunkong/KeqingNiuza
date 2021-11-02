using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 间
    /// </summary>
    public class Level
    {
        [JsonProperty("index")] public int Index { get; set; }
        [JsonProperty("star")] public int Star { get; set; }
        [JsonProperty("max_star")] public int MaxStar { get; set; }
        [JsonProperty("battles")] public List<Battle> Battles { get; set; }
    }
}
