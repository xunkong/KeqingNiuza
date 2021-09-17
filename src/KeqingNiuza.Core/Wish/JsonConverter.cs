using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    class GachaTypeJsonConverter : JsonConverter<WishType>
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

    class ItemIdJsonConverter : JsonConverter<int>
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

    class TimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
#if INTERNAIONAL
            var str = reader.GetString();
#else
            var str = reader.GetString() + " +08:00";
#endif
            return DateTime.Parse(str);

        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
#if INTERNAIONAL
            var time = value;
#else
            var time = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
#endif
            writer.WriteStringValue(time.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }



}
