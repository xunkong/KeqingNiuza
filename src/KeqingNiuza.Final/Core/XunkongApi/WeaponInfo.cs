using KeqingNiuza.Core.Wish;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.XunkongApi
{
    internal class WeaponInfo
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enable { get; set; }

        public string Description { get; set; }

        public long NameTextMapHash { get; set; }

        public long DescTextMapHash { get; set; }

        public int Rarity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WeaponType WeaponType { get; set; }

        public string Icon { get; set; }

        public string AwakenIcon { get; set; }

        public string GachaIcon { get; set; }


        [JsonIgnore]
        public string ToThumb => $"Resource/Weapon/{Path.GetFileName(Icon)}";
        [JsonIgnore]
        public string ToPortrait => $"Resource/Weapon/{Path.GetFileName(GachaIcon)}";


    }
}
