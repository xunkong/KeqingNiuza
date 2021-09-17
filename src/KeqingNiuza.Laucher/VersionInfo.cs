using System;
using System.Collections.Generic;

namespace KeqingNiuza.Launcher
{
    class VersionInfo
    {
        public string Version { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public List<KeqingNiuzaFileInfo> KeqingNiuzaFiles { get; set; }
    }


}
