using System;
using System.IO;

namespace KeqingNiuza.Core.CloudBackup
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
