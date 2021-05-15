using KeqingNiuza.Model;
using KeqingNiuza.Wish;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KeqingNiuza.ViewModel
{
    public class WishAchievementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public FontFamily Hanyi { get; set; }

        public WishAchievementViewModel(UserData userData)
        {
            var analyzer = new AchievementAnalyzer(userData.WishLogFile);
            AchievementInfoList = analyzer.AchievementList;
        }




        private List<AchievementInfo> _AchievementInfoList;
        public List<AchievementInfo> AchievementInfoList
        {
            get { return _AchievementInfoList; }
            set
            {
                _AchievementInfoList = value;
                OnPropertyChanged();
            }
        }


    }
}
