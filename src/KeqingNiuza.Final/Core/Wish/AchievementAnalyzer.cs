using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KeqingNiuza.Core.Wish
{
    public class AchievementAnalyzer
    {

        private readonly List<WishData> _WishDataList;

        public List<AchievementInfo> AchievementList { get; set; }

        public AchievementAnalyzer(string path)
        {
            _WishDataList = LocalWishLogLoader.Load(path);
            Analyzer();
        }

        public AchievementAnalyzer(List<WishData> list)
        {
            _WishDataList = list;
            Analyzer();
        }


        public void Analyzer()
        {
            var tempList = new List<AchievementInfo>();
            var methods = typeof(AchievementComputeMethod).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                method.Invoke(null, new object[] { _WishDataList, tempList });
            }
            AchievementList = tempList.OrderByDescending(x => x.IsFinished).ThenByDescending(x => x.FinishTime).ToList();
        }
    }
}
