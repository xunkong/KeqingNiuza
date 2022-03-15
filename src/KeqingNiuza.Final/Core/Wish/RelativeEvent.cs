using System;
using System.Collections.Generic;

namespace KeqingNiuza.Core.Wish
{
    public class RelativeEvent
    {
        public WishEvent WishEvent { get; set; }

        public List<WishData> WishDataList { get; set; }

        public string EventName { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string UpItems { get; set; }

        public int TotalCount { get; set; }

        public int ThisCount { get; set; }

        public int Count_XiaoBaoDi { get; set; }

        public int Count_DaBaoDi { get; set; }
    }
}
