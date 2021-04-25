namespace KeqingNiuza.Wish
{
    public struct QueryParam
    {
        public WishType WishType { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public long EndId { get; set; }

        public override string ToString()
        {
            return $@"gacha_type={(int)WishType}&page={Page}&size={Size}&end_id={EndId}";
        }
    }
}
