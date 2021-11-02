using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class JourneyDetail : JourneyBase
    {
        [JsonProperty("page")] public int Page { get; set; }
        [JsonProperty("list")] public List<DetailAction> List { get; set; }
    }

}
