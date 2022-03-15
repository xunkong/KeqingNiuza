using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    class ResponseData
    {
        [JsonPropertyName("retcode")]
        public int Retcode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public DataItem Data { get; set; }

        public class DataItem
        {
            [JsonPropertyName("page"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int Page { get; set; }

            [JsonPropertyName("size"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int Size { get; set; }

            [JsonPropertyName("total"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int Total { get; set; }

            [JsonPropertyName("list")]
            public List<WishData> List { get; set; }

            [JsonPropertyName("region")]
            public string Region { get; set; }
        }
    }
}
