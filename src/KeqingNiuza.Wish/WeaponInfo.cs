using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeqingNiuza.Wish
{
    public class WeaponInfo : ItemInfo
    {


        public WeaponType WeaponType { get; set; }


        [JsonIgnore]
        public override string Thumb => $"resource\\weapon\\Weapon_{NameEn.Replace(" ", "_")}.png";

        [JsonIgnore]
        public override string Portrait => $"resource\\weapon\\Weapon_{NameEn.Replace(" ", "_")}.png";
    }
}
