using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KeqingNiuza.RealtimeNotes
{
    public static class Endecryption
    {

        private static readonly byte[] Key;

        private static readonly byte[] IV;

        static Endecryption()
        {
            var UserName = Environment.UserName;
            var MachineGuid = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\", "MachineGuid", UserName);
            var hash = SHA256.Create();
            var byte1 = Encoding.UTF8.GetBytes(MachineGuid + UserName);
            var byte2 = Encoding.UTF8.GetBytes(UserName);
            Key = hash.ComputeHash(byte1);
            IV = hash.ComputeHash(byte2).Take(16).ToArray();
        }


        public static byte[] Encrypt(string str)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(str);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }


        public static string Decrypt(byte[] bytes)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }


    }
}
