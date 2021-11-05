using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
//using System.Text.Encodings.Web;
//using System.Text.Json;
//using System.Text.Unicode;

namespace KeqingNiuza.RealtimeNotes
{
    internal class Const
    {
        //public static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions() { AllowTrailingCommas = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true };

#if PACKAGED
        public static string UserDataPath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\RealtimeNotes\\UserData";
#else
        public static string UserDataPath { get; } = "..\\UserData";
#endif

#if PACKAGED
        public static string LogPath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\RealtimeNotes\\Log";
#else
        public static string LogPath { get; } = "..\\Log";
#endif

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


        private static string fileVersion;

        public static string FileVersion
        {
            get
            {
                if (fileVersion != null)
                {
                    return fileVersion;
                }
                var name = typeof(Const).Assembly.Location;
                var v = FileVersionInfo.GetVersionInfo(name);
                fileVersion = v.FileVersion;
                return fileVersion;
            }
        }



        public static string CookiesFile => $@"{UserDataPath}\DailyCheckCookies";
    }
}
