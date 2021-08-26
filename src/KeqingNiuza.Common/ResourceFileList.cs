using System;
using System.Collections.Generic;

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
