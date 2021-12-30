using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeqingNiuza.Core.Wish;

namespace KeqingNiuza.Model
{
    internal class WishlogModel
    {
        public int Uid { get; set; }


        public string Url { get; set; }


        public long LastId { get; set; }


        public IEnumerable<WishData> List { get; set; }
    }
}
