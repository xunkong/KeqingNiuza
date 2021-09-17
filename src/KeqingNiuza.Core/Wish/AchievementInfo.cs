using System;

namespace KeqingNiuza.Core.Wish
{
    /// <summary>
    /// 单条成就信息
    /// </summary>
    public class AchievementInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 额外评论
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// 未完成进度
        /// </summary>
        public string Progress { get; set; }

        /// <summary>
        /// 已完成溢出内容
        /// </summary>
        public string Total { get; set; }
    }
}
