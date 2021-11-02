using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class JourneyBase
    {
        [JsonProperty("uid")] public int Uid { get; set; }//
        [JsonProperty("region")] public string Region { get; set; }//
        [JsonProperty("account_id")] public int AccountId { get; set; }//
        [JsonProperty("nickname")] public string Nickname { get; set; }//
        [JsonProperty("date")] public string Date { get; set; }//
        [JsonProperty("month")] public int Month { get; set; }//
        [JsonProperty("optional_month")] public List<int> OptionalMonth { get; set; }//
        [JsonProperty("data_month")] public int DataMonth { get; set; }//
    }
}