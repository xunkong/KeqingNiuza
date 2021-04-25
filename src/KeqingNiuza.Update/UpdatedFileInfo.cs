using System;

namespace KeqingNiuza.Update
{
    public class UpdatedFileInfo : IEquatable<UpdatedFileInfo>
    {
        public string Name { get; set; }

        /// <summary>
        /// 文件路径，以根目录文件夹为基准
        /// </summary>
        public string Path { get; set; }


        public string SHA256 { get; set; }

        /// <summary>
        /// 0 无变化，1 替换，-1删除
        /// </summary>
        public int Mode { get; set; }


        public UpdatedFileInfo() { }


        public UpdatedFileInfo(UpdatedFileInfo info)
        {
            Name = info.Name;
            Path = info.Path;
            SHA256 = info.SHA256;
            Mode = info.Mode;
        }


        public bool Equals(UpdatedFileInfo other)
        {
            return other.Path == Path && other.SHA256 == SHA256;
        }


        public override int GetHashCode()
        {
            return (Path, SHA256).GetHashCode();
        }

    }


}
