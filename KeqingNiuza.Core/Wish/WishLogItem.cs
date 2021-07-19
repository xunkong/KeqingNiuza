using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.Wish
{
    public class WishLogItem
    {
        /// <summary>
        /// 用户Uid
        /// </summary>
        [JsonPropertyName("uid"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public int Uid { get; set; }

        /// <summary>
        /// 祈愿类型（卡池类型）
        /// </summary>
        [JsonPropertyName("gacha_type"), JsonConverter(typeof(WishTypeJsonConverter))]
        public WishType WishType { get; set; }

        /// <summary>
        /// 此值为空
        /// </summary>
        [JsonPropertyName("item_id"), JsonConverter(typeof(ItemIdJsonConverter))]
        public int ItemId { get; set; }

        /// <summary>
        /// 物品数量（暂时都是1）
        /// </summary>
        [JsonPropertyName("count"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public int Count { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(TimeJsonConverter))]
        public DateTime Time { get; set; }

        /// <summary>
        /// 物品名称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 语言（如zh-cn）
        /// </summary>
        [JsonPropertyName("lang")]
        public string Language { get; set; }

        /// <summary>
        /// 物品类型（角色、武器）
        /// </summary>
        [JsonPropertyName("item_type")]
        public string ItemType { get; set; }

        /// <summary>
        /// 星级
        /// </summary>
        [JsonPropertyName("rank_type"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public int Rank { get; set; }

        /// <summary>
        /// 祈愿Id，这个值很重要，全服唯一
        /// </summary>
        [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public long Id { get; set; }


#if false


        /// <summary>
        /// 是否丢失祈愿Id，对应从其他软件导出数据，如果丢失，则祈愿Id的值为本地生成，不是全服唯一
        /// </summary>
        public bool IsLostId { get; set; }

        /// <summary>
        /// 在相应卡池中的序号
        /// </summary>
        [JsonIgnore]
        public int Number { get; set; }

        /// <summary>
        /// 是否Up
        /// </summary>
        [JsonIgnore]
        public bool? IsUp { get; set; }

        /// <summary>
        /// 保底内次数，此值不保存，需每次重新计算
        /// </summary>
        [JsonIgnore]
        public int Guarantee { get; set; }

        /// <summary>
        /// 保底类型，小保底还是大保底
        /// </summary>
        [JsonIgnore]
        public string GuaranteeType { get; set; }

        [JsonIgnore]
        private static List<CharacterInfo> CharacterInfoList = Const.CharacterInfoList;

        [JsonIgnore]
        private static List<WeaponInfo> WeaponInfoList = Const.WeaponInfoList;


        [JsonIgnore]
        public string Thumb
        {
            get
            {
                if (ItemType == "角色")
                {
                    return CharacterInfoList.Where(x => x.Name == Name).Select(x => x.Thumb).First();
                }
                if (ItemType == "武器")
                {
                    return WeaponInfoList.Where(x => x.Name == Name).Select(x => x.Thumb).First();
                }
                return null;
            }
        }


        [JsonIgnore]
        public string Portrait
        {
            get
            {
                if (ItemType == "角色")
                {
                    return CharacterInfoList.Where(x => x.Name == Name).Select(x => x.Portrait).First();
                }
                if (ItemType == "武器")
                {
                    return WeaponInfoList.Where(x => x.Name == Name).Select(x => x.Portrait).First();
                }
                return null;
            }
        }


#endif



        public bool Equals(WishLogItem other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    internal class TimeJsonConverter : JsonConverter<WishType>
    {
        public override WishType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (WishType)int.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, WishType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(((int)value).ToString());
        }
    }

    internal class ItemIdJsonConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var src = reader.GetString();
            if (src == "")
            {
                return 0;
            }
            else
            {
                return int.Parse(src);
            }
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            if (value == 0)
            {
                writer.WriteStringValue("");
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }

        }
    }

    internal class WishTypeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            //var str = reader.GetString() + " +08:00";
            return DateTime.Parse(str);

        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var time = value;
            //var time = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            writer.WriteStringValue(time.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
