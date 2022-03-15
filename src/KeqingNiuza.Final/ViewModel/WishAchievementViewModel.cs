using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KeqingNiuza.Core.Wish;
using KeqingNiuza.Model;

namespace KeqingNiuza.ViewModel
{
    public class WishAchievementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public WishAchievementViewModel(UserData userData)
        {
            var analyzer = new AchievementAnalyzer(userData.WishLogFile);
            AchievementInfoList = analyzer.AchievementList;
        }


        public WishAchievementViewModel()
        {
            var analyzer = new AchievementAnalyzer(MainWindowViewModel.WishDataList);
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
