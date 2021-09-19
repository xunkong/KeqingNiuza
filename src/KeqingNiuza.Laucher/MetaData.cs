using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace KeqingNiuza.Launcher
{
    internal class MetaData
    {
        private static string fileVersion;


        public static string FileVersion
        {
            get
            {
                if (fileVersion != null)
                {
                    return fileVersion;
                }
                var name = typeof(MetaData).Assembly.Location;
                var v = FileVersionInfo.GetVersionInfo(name);
                fileVersion = v.FileVersion;
                return fileVersion;
            }
        }



        private static string userId;

        public static string UserId
        {
            get
            {
                if (userId != null)
                {
                    return userId;
                }
                var UserName = Environment.UserName;
                var MachineGuid = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\", "MachineGuid", UserName);
                var bytes = Encoding.UTF8.GetBytes(UserName + MachineGuid);
                var hash = MD5.Create().ComputeHash(bytes);
                userId = BitConverter.ToString(hash).Replace("-", "");
                return userId;
            }
        }
    }
}
