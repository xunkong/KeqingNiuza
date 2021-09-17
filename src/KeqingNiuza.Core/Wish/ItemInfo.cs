using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    public class ItemInfo
    {
        public string Name { get; set; }

        public string NameEn { get; set; }

        public int Rank { get; set; }

        public string ItemType { get; set; }

        [JsonIgnore]
        public int Count { get; set; }

        [JsonIgnore]
        public DateTime LastGetTime { get; set; }

        [JsonIgnore]
        public List<WishData> WishDataList { get; set; }

        [JsonIgnore]
        public virtual string Thumb { get; set; }

        [JsonIgnore]
        public virtual string Portrait { get; set; }
    }


}
