using System;
using System.IO;

namespace KeqingNiuza.Common
{
    public class UpdateFileInfo : IEquatable<UpdateFileInfo>
    {

        public string Name { get; set; }


        public string Path { get; set; }


        public long Size { get; set; }


        public string SHA256 { get; set; }

        public UpdateFileInfo() { }

        public UpdateFileInfo(string path)
        {
            var fileInfo = new FileInfo(path);
            Name = fileInfo.Name;
            var dir = Environment.CurrentDirectory;
            Path = fileInfo.FullName.Replace(dir + "\\", "");
            Size = fileInfo.Length;
            SHA256 = Util.GetFileHash(fileInfo.FullName);
        }


        public UpdateFileInfo(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            var dir = Environment.CurrentDirectory;
            Path = fileInfo.FullName.Replace(dir + "\\", "");
            Size = fileInfo.Length;
            SHA256 = Util.GetFileHash(fileInfo.FullName);
        }

        public bool Equals(UpdateFileInfo other)
        {
            return other.Path == Path && other.SHA256 == SHA256;
        }

        public override int GetHashCode()
        {
            return (Path, SHA256).GetHashCode();
        }
    }
}
