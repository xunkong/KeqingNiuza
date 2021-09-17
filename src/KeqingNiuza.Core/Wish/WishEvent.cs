using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    public class WishEvent
    {
        public WishType WishType { get; set; }
        public string Name { get; set; }
        public float Version { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> UpStar5 { get; set; }
        public List<string> UpStar4 { get; set; }

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                return $"{Version:F1} {Name}";
            }
        }

        [JsonIgnore]
        public string UpItems
        {
            get
            {
                string result = "";
                foreach (var item in UpStar5)
                {
                    result += $" {item}";
                }
                foreach (var item in UpStar4)
                {
                    result += $" {item}";
                }
                return result.Trim();
            }
        }
    }
}
