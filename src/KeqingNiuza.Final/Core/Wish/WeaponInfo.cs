using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    public class WeaponInfo : ItemInfo
    {


        public WeaponType WeaponType { get; set; }

        public override string ItemType { get; set; } = "武器";

    }
}
