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
    internal class CharacterInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enable { get; set; }

        public long NameTextMapHash { get; set; }

        public long DescTextMapHash { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Rarity { get; set; }

        public string Gender { get; set; }

        /// <summary>
        /// 所属势力
        /// </summary>
        public string Affiliation { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementType Element { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WeaponType WeaponType { get; set; }

        /// <summary>
        /// MM/dd
        /// </summary>
        public string Birthday { get; set; }

        public string Card { get; set; }

        public string Portrait { get; set; }

        public string FaceIcon { get; set; }

        public string SideIcon { get; set; }

        public string GachaCard { get; set; }

        public string GachaSplash { get; set; }

        public string AvatarIcon { get; set; }

        public string ConstllationName { get; set; }

        [JsonIgnore]
        public string ToThumb => $"Resource/Character/{Path.GetFileName(FaceIcon)}";

        [JsonIgnore]
        public string ToPortrait => $"Resource/Character/{Path.GetFileName(GachaSplash)}";
    }
}
