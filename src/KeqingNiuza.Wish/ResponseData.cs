using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Wish { 
    class ResponseData
    {
        public int retcode { get; set; }

        public string message { get; set; }

        public Data data { get; set; }

        public class Data
        {
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int page { get; set; }

            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int size { get; set; }

            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int total { get; set; }

            public List<WishData> list { get; set; }

            public string region { get; set; }
        }
    }
}
