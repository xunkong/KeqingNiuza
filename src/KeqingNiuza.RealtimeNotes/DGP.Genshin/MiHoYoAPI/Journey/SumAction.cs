using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class SumAction : JourneyAction
    {
        [JsonProperty("percent")] public int Percent { get; set; }
    }
}
