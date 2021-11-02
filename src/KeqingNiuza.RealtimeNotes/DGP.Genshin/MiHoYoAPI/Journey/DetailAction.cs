using Newtonsoft.Json;
using System;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class DetailAction : JourneyAction
    {
        [JsonProperty("time")] public DateTime Percent { get; set; }
    }

}
