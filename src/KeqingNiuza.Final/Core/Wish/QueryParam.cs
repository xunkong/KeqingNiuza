namespace KeqingNiuza.Core.Wish
{
    public struct QueryParam
    {
        /// <summary>
        /// 祈愿类型
        /// </summary>
        public WishType WishType { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 请求数据量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 上一页最后一条的id
        /// </summary>
        public long EndId { get; set; }

        public override string ToString()
        {
            return $@"gacha_type={(int)WishType}&page={Page}&size={Size}&end_id={EndId}";
        }
    }
}
