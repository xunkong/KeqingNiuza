using Newtonsoft.Json;
using System;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class JourneyInfo : JourneyBase
    {
        [JsonProperty("data_last_month")] public int DataLastMonth { get; set; }
        [JsonProperty("day_data")] public Day DayData { get; set; }
        [JsonProperty("month_data")] public Month MonthData { get; set; }
        [Obsolete] [JsonProperty("lantern")] public bool Lantern { get; set; }
    }
}
