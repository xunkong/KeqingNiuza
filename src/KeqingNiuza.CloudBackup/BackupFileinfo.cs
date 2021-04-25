using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDav;

namespace KeqingNiuza.CloudBackup
{
    class BackupFileinfo
    {
        public string Name { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public BackupFileinfo(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            LastUpdateTime = fileInfo.LastWriteTime;
        }

        public BackupFileinfo() { }

    }
}
