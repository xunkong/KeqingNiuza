using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Common
{
    public class ResourceFileInfo : IEquatable<ResourceFileInfo>
    {
        public string Name { get; set; }


        public string Url { get; set; }


        public string Path { get; set; }


        public long Size { get; set; }


        public string SHA256 { get; set; }

        public ResourceFileInfo() { }


        public ResourceFileInfo(string path)
        {
            var fileInfo = new FileInfo(path);
            Name = fileInfo.Name;
            var dir = Environment.CurrentDirectory;
            Path = fileInfo.FullName.Replace(dir + "\\", "");
            Size = fileInfo.Length;
            SHA256 = Util.GetFileHash(fileInfo.FullName);
            Url = new Uri(_baseUri, Path).ToString();
        }


        public ResourceFileInfo(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            var dir = Environment.CurrentDirectory;
            Path = fileInfo.FullName.Replace(dir + "\\", "");
            Size = fileInfo.Length;
            SHA256 = Util.GetFileHash(fileInfo.FullName);
            Url = new Uri(_baseUri, Path).ToString();
        }

        private static readonly Uri _baseUri = new Uri("https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/");

        public bool Equals(ResourceFileInfo other)
        {
            return other.Path == Path && other.SHA256 == SHA256;
        }

        public override int GetHashCode()
        {
            return (Path, SHA256).GetHashCode();
        }



    }
}
