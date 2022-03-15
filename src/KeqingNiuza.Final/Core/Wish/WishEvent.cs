using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    public class WishEvent
    {
        public WishType WishType { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> Rank5UpItems { get; set; }
        public List<string> Rank4UpItems { get; set; }

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                return $"{Version} {Name}";
            }
        }

        [JsonIgnore]
        public string UpItems
        {
            get
            {
                string result = "";
                foreach (var item in Rank5UpItems)
                {
                    result += $" {item}";
                }
                foreach (var item in Rank4UpItems)
                {
                    result += $" {item}";
                }
                return result.Trim();
            }
        }
    }
}
