using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class Month
    {
        [JsonProperty("current_primogems")] public int CurrentPrimogems { get; set; }
        [JsonProperty("current_mora")] public int CurrentMora { get; set; }
        [JsonProperty("last_primogems")] public int LastPrimogems { get; set; }
        [JsonProperty("last_mora")] public int LastMora { get; set; }
        [JsonProperty("current_primogems_level")] public int CurrentPrimogemsLevel { get; set; }
        [JsonProperty("primogems_rate")] public int PrimogemsRate { get; set; }
        [JsonProperty("mora_rate")] public int MoraRate { get; set; }
        [JsonProperty("group_by")] public List<SumAction> GroupBy { get; set; }
    }
}
