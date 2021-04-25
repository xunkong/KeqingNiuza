using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Update
{
    class FileList
    {
        /// <summary>
        /// 项目版本
        /// </summary>
        [JsonConverter(typeof(VersionJsonConverter))]
        public Version Version { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 版本说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否自动更新
        /// </summary>
        public bool AutoUpdate { get; set; } = true;

        /// <summary>
        /// 压缩包下载地址
        /// </summary>
        public string PackageUrl { get; set; }

        /// <summary>
        /// 压缩包文件名
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 压缩包的SHA256值
        /// </summary>
        public string PackageSHA256 { get; set; }

        /// <summary>
        /// 所有文件的信息
        /// </summary>
        public List<UpdatedFileInfo> AllFiles { get; set; }


        /// <summary>
        /// 需要更新的文件的信息
        /// </summary>
        public List<UpdatedFileInfo> UpdatedFiles { get; set; }


        /// <summary>
        /// 历史版本记录
        /// </summary>
        public List<FileList> VersionHistory { get; set; }


        public FileList()
        {
            UpdatedFiles = new List<UpdatedFileInfo>();
            VersionHistory = new List<FileList>();
        }

        public FileList(FileList fileList)
        {
            Version = fileList.Version;
            PackageUrl = fileList.PackageUrl;
            UpdateTime = fileList.UpdateTime;
            Description = fileList.Description;
            //AllFiles = fileList.AllFiles.ConvertAll(x => new UpdatedFileInfo(x));
            UpdatedFiles = fileList.UpdatedFiles.ConvertAll(x => new UpdatedFileInfo(x));
        }
    }
}
