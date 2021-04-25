namespace KeqingNiuza.Wish
{
    public class StarDetail
    {
        public string Name { get; set; }

        public int Num { get; set; }

        public string Time { get; set; }

        public string Brush => Const.ElementDictionary.ContainsKey(Name) ? Const.ElementDictionary[Name] : "#000000";


        public StarDetail(string name, int num, string time)
        {
            Name = name;
            Num = num;
            Time = time;
        }
    }
}
