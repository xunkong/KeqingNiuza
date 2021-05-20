using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Common
{
    public class UpdateFileList
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
        public bool AutoUpdate { get; set; }

        /// <summary>
        /// 显示更新完提示
        /// </summary>
        public bool ShowUpdateLogView { get; set; }

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
        public List<UpdateFileInfo> AllFiles { get; set; }



    }
}
