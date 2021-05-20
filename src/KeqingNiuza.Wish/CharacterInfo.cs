using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeqingNiuza.Wish
{
    public class CharacterInfo : ItemInfo
    {

        public ElementType ElementType { get; set; }


        [JsonIgnore]
        public override string Thumb => $"resource\\character\\Character_{NameEn.Replace(" ", "_")}_Thumb.png";


        [JsonIgnore]
        public override string Portrait => $"resource\\character\\Character_{NameEn.Replace(" ", "_")}_Portrait.png";
    }
}
