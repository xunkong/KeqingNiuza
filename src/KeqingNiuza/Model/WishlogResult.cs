using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeqingNiuza.Core.Wish;

namespace KeqingNiuza.Model
{
    internal class WishlogResult
    {
        public int Uid { get; set; }

        public int CurrentCount { get; set; }

        public int GetCount { get; set; }

        public int PutCount { get; set; }

        public int DeleteCount { get; set; }

        public List<WishData> List { get; set; }

    }
}
