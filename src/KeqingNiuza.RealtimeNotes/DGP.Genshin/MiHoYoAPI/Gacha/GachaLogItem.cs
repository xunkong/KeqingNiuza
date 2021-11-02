using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    /// <summary>
    /// 表示获取的单个卡池物品信息
    /// </summary>
    public class GachaLogItem
    {
        [JsonProperty("uid")] public string Uid { get; set; }
        [JsonProperty("gacha_type")] public string GachaType { get; set; }
        [Obsolete("api removed this property")] [JsonProperty("item_id")] public string ItemId { get; set; }
        [JsonProperty("count")] public string Count { get; set; }//Always 1
        [JsonProperty("time")] public DateTime Time { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("lang")] public string Language { get; set; }//zh-cn
        [JsonProperty("item_type")] public string ItemType { get; set; }
        [JsonProperty("rank_type")] public string Rank { get; set; }
        [JsonProperty("id")] public string Id { get; set; }

        [JsonIgnore]
        public long TimeId
        {
            get
            {
                Debug.Assert(Id != null);
                return long.Parse(Id);
            }
        }
    }
}
