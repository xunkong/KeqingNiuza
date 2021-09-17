using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
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
