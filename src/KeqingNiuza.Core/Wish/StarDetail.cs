namespace KeqingNiuza.Core.Wish
{
    public class StarDetail
    {
        public string Name { get; set; }

        public int Num { get; set; }

        public string Time { get; set; }

        public string Brush { get; set; }


        public StarDetail(string name, int num, string time)
        {
            Name = name;
            Num = num;
            Time = time;
        }

        public StarDetail() { }
    }
}
