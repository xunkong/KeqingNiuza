using System;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Wish
{
    public class WishData : IEquatable<WishData>
    {
        [JsonPropertyName("uid"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public int Uid { get; set; }

        [JsonPropertyName("gacha_type"), JsonConverter(typeof(GachaTypeJsonConverter))]
        public WishType WishType { get; set; }

        [JsonPropertyName("item_id"), JsonConverter(typeof(ItemIdJsonConverter))]
        public int ItemId { get; set; }

        [JsonPropertyName("count"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public int Count { get; set; }

        [JsonPropertyName("time"), JsonConverter(typeof(TimeJsonConverter))]
        public DateTime Time { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lang")]
        public string Language { get; set; }

        [JsonPropertyName("item_type")]
        public string ItemType { get; set; }

        [JsonPropertyName("rank_type"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public int RankType { get; set; }

        [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public long Id { get; set; }

        public bool Equals(WishData other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
