using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.CloudBackup
{
    class AccountInfo
    {
        public string Url { get; set; }

        public CloudType CloudType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime LastSyncTime { get; set; }

    }
}
