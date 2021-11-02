using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class Day
    {
        [JsonProperty("current_primogems")] public int CurrentPrimogems { get; set; }
        [JsonProperty("current_mora")] public int CurrentMora { get; set; }
        [JsonProperty("last_primogems")] public int LastPrimogems { get; set; }
        [JsonProperty("last_mora")] public int LastMora { get; set; }
    }
}
