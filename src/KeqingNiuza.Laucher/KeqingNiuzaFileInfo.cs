using System;
using System.IO;

namespace KeqingNiuza.Launcher
{
    class KeqingNiuzaFileInfo : IEquatable<KeqingNiuzaFileInfo>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public long CompressedSize { get; set; }

        public string SHA256 { get; set; }

        public string Url { get; set; }

        public string ContentEncoding { get; set; }


        public KeqingNiuzaFileInfo() { }


        public KeqingNiuzaFileInfo(string path)
        {
            Path = System.IO.Path.GetFullPath(path);
            Name = System.IO.Path.GetFileName(path);
            var bytes = File.ReadAllBytes(path);
            Size = bytes.LongLength;
            SHA256 = Util.HashData(bytes);
        }


        public static KeqingNiuzaFileInfo Create(string path)
        {
            var result = new KeqingNiuzaFileInfo();
            var bytes = File.ReadAllBytes(path);
            result.Path = path;
            result.Name = System.IO.Path.GetFileName(path);
            result.Size = bytes.LongLength;
            result.SHA256 = Util.HashData(bytes);
            result.ContentEncoding = "Deflate";
            return result;
        }

        public bool Equals(KeqingNiuzaFileInfo other)
        {
            return (Path, SHA256) == (other?.Path, other?.SHA256);
        }


        public override int GetHashCode()
        {
            return (Path, SHA256).GetHashCode();
        }
    }
}
