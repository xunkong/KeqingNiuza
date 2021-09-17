using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Service
{
    internal class Const
    {
        public static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true };

        public static string UserDataPath { get; } = "..\\UserData";

        public static string LogPath { get; } = "..\\Log";

        private static string _userId;

        public static string UserId
        {
            get
            {
                if (_userId != null)
                {
                    return _userId;
                }
                var UserName = Environment.UserName;
                var MachineGuid = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\", "MachineGuid", UserName);
                var bytes = Encoding.UTF8.GetBytes(UserName + MachineGuid);
                var hash = MD5.Create().ComputeHash(bytes);
                _userId = BitConverter.ToString(hash).Replace("-", "");
                return _userId;
            }
        }
    }
}
