using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class JourneyAction
    {
        [JsonProperty("action_id")] public int ActionId { get; set; }
        [JsonProperty("action")] public string Action { get; set; }
        [JsonProperty("num")] public int Num { get; set; }
    }
}