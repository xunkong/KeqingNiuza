using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    public class CharacterInfo : ItemInfo
    {

        public ElementType ElementType { get; set; }

        public override string ItemType { get; set; } = "角色";

    }
}
