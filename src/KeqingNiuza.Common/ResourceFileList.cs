using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Common
{
    public class ResourceFileList
    {
        public DateTime LastUpdateTime { get; set; }

        public int FileCount { get; set; }

        public long TotalSize { get; set; }

        public List<ResourceFileInfo> Files { get; set; }

    }
}
