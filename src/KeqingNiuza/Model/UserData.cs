using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KeqingNiuza.Model
{
    public class UserData : INotifyPropertyChanged
    {
        public int Uid { get; set; }

        public string Url { get; set; }

        public string WishLogFile => $"UserData\\WishLog_{Uid}.json";


        private DateTime _LastUpdateTime;
        public DateTime LastUpdateTime
        {
            get { return _LastUpdateTime; }
            set
            {
                _LastUpdateTime = value;
                OnPropertyChanged();
            }
        }

        private string _Avatar = "/resource/avatar/Paimon.png";
        public string Avatar
        {
            get { return _Avatar; }
            set
            {
                _Avatar = value;
                OnPropertyChanged();
            }
        }


#warning 以后记得删除
        private bool _HiddenNoviceWish;
        public bool HiddenNoviceWish
        {
            get { return _HiddenNoviceWish; }
            set
            {
                _HiddenNoviceWish = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
