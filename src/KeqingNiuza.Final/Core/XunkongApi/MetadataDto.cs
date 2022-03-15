using System.Collections.Generic;

namespace KeqingNiuza.Core.XunkongApi
{
    internal class MetadataDto<T> where T : class
    {

        public int Count { get; set; }


        public List<T> List { get; set; }

    }
}
