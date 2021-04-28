using System.Collections.Generic;

namespace KeqingNiuza.Update
{
    class UpdatedFileList
    {
        /// <summary>
        /// 更新文件所在文件夹
        /// </summary>
        public string SourceDirPath { get; set; }

        /// <summary>
        /// 是否更新完成
        /// </summary>
        public bool IsUpdateFinished { get; set; }

        public bool ShowUpdateLogView { get; set; }

        /// <summary>
        /// 需要更新的文件
        /// </summary>
        public List<UpdatedFileInfo> UpdatedFiles { get; set; }

        /// <summary>
        /// 更新失败的文件
        /// </summary>
        public List<UpdatedFileInfo> FailedFiles { get; set; }

        public UpdatedFileList()
        {
            UpdatedFiles = new List<UpdatedFileInfo>();
            FailedFiles = new List<UpdatedFileInfo>();
        }
    }
}
