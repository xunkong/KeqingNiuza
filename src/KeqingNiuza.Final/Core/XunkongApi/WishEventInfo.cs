using KeqingNiuza.Core.Wish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.XunkongApi
{
    internal class WishEventInfo
    {



        public int Id { get; set; }


        public WishType WishType { get; set; }

        public WishType QueryType => WishTypeToQueryType(WishType);

        public string Name { get; set; }

        public string Version { get; set; }

        [JsonIgnore]
        public DateTime StartTime => TimeStringToTimeOffset(_StartTimeString);


        [JsonIgnore]
        public DateTime EndTime => TimeStringToTimeOffset(_EndTimeString);


        [JsonPropertyName("StartTime"), JsonInclude]
        public string _StartTimeString { get; private set; }


        [JsonPropertyName("EndTime"), JsonInclude]
        public string _EndTimeString { get; private set; }

        /// <summary>
        /// Up5星物品，此值不要使用SQL查询
        /// </summary>
        public List<string> Rank5UpItems { get; set; }

        /// <summary>
        /// Up4星物品，此值不要使用SQL查询
        /// </summary>
        public List<string> Rank4UpItems { get; set; }




        private WishType WishTypeToQueryType(WishType type)
        {
            switch (type)
            {

                case WishType.CharacterEvent_2:
                    return WishType.CharacterEvent;
                default:
                    return type;
            }
        }




        private static DateTime TimeStringToTimeOffset(string str)
        {
            if (str.Contains("+"))
            {
                var time = DateTime.Parse(str.Split('+')[0]);
                var newTime = time.AddDays(-1);
                return new DateTime(newTime.Year, newTime.Month, newTime.Day, 20, 0, 0);
            }
            else
            {
                return DateTime.Parse(str);
            }
        }

    }
}
