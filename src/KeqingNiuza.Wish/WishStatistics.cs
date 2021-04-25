using System;
using System.Collections.Generic;

namespace KeqingNiuza.Wish
{

    public class WishStatistics
    {
        /// <summary>
        /// 祈愿类型
        /// </summary>
        public WishType WishType { get; set; }

        /// <summary>
        /// 第一抽时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 最后一抽时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 祈愿总数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 累计未出5星
        /// </summary>
        public int Guarantee { get; set; }

        /// <summary>
        /// 小保底还是大保底
        /// </summary>
        public string GuaranteeType { get; set; }

        /// <summary>
        /// 5星总数
        /// </summary>
        public int Star5Count { get; set; }

        /// <summary>
        /// 4星总数
        /// </summary>
        public int Star4Count { get; set; }

        /// <summary>
        /// 3星总数
        /// </summary>
        public int Star3Count { get; set; }

        /// <summary>
        /// 5星角色数
        /// </summary>
        public int Character5Count { get; set; }

        /// <summary>
        /// 4星角色数
        /// </summary>
        public int Character4Count { get; set; }

        /// <summary>
        /// 5星武器数
        /// </summary>
        public int Weapon5Count { get; set; }

        /// <summary>
        /// 4星武器数
        /// </summary>
        public int Weapon4Count { get; set; }

        /// <summary>
        /// 3星武器数
        /// </summary>
        public int Weapon3Count { get; set; }

        /// <summary>
        /// 5星概率
        /// </summary>
        public double Ratio5 => (double)Star5Count / Count;

        /// <summary>
        /// 4星概率
        /// </summary>
        public double Ratio4 => (double)Star4Count / Count;

        /// <summary>
        /// 3星概率
        /// </summary>
        public double Ratio3 => (double)Star3Count / Count;

        /// <summary>
        /// 5星平均出货次数
        /// </summary>
        public double Average5 => (double)(Count - Guarantee) / Star5Count;

        /// <summary>
        /// 5星详细列表
        /// </summary>
        public List<StarDetail> Star5List { get; set; }

        /// <summary>
        /// 4星详细列表
        /// </summary>
        public List<StarDetail> Star4List { get; set; }


    }
}
