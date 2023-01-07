using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    public class ItemInfo
    {
        public string Name { get; set; }

        public int Rank { get; set; }

        public virtual string ItemType { get; set; }

        [JsonIgnore]
        public int Count { get; set; }

        [JsonIgnore]
        public DateTime LastGetTime { get; set; }

        [JsonIgnore]
        public List<WishData> WishDataList { get; set; }

        public string Thumb { get; set; }

        public string NameEn { get; set; }

        public string Portrait { get; set; }
    }


}
