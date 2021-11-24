using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using KeqingNiuza.Core.Wish;
using KeqingNiuza.Model;

namespace KeqingNiuza.ViewModel
{
    public class WishOriginalDataViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Item Source

        public List<WishData> WishDataList { get; set; }


        private List<WishData> _FilteredWishData;
        public List<WishData> FilteredWishData
        {
            get { return _FilteredWishData; }
            set
            {
                _FilteredWishData = value;
                OnPropertyChanged();
            }
        }


        public List<string> WishTypeList { get; set; }

        private string _SelectedWishType = "---";

        public string SelectedWishType
        {
            get { return _SelectedWishType; }
            set
            {
                _SelectedWishType = value;
                OnPropertyChanged();
                FilterWishData();
            }
        }

        public List<string> ItemTypeList { get; set; }

        private string _SelectedItemType = "-";
        public string SelectedItemType
        {
            get { return _SelectedItemType; }
            set
            {
                _SelectedItemType = value;
                OnPropertyChanged();
                FilterWishData();
            }
        }



        public List<string> ItemRankList { get; set; }

        private string _SelectedItemRank = "-";
        public string SelectedItemRank
        {
            get { return _SelectedItemRank; }
            set
            {
                _SelectedItemRank = value;
                OnPropertyChanged();
                FilterWishData();
            }
        }

        public List<WishEvent> WishEventList { get; set; }

        private WishEvent _SelectedWishEvent;
        public WishEvent SelectedWishEvent
        {
            get { return _SelectedWishEvent; }
            set
            {
                _SelectedWishEvent = value;
                OnPropertyChanged();
                SelectedWishType = "---";
                FilterWishData();
            }
        }

        #endregion


        #region Control Property


        private bool _ToggleButton_Search_IsChecked;
        public bool ToggleButton_Search_IsChecked
        {
            get { return _ToggleButton_Search_IsChecked; }
            set
            {
                _ToggleButton_Search_IsChecked = value;
                OnPropertyChanged();
                FilterWishData();
            }
        }

        private string _TextBox_Search_Text = "";
        public string TextBox_Search_Text
        {
            get { return _TextBox_Search_Text; }
            set
            {
                _TextBox_Search_Text = value;
                OnPropertyChanged();
                FilterWishData();
            }
        }


        #endregion


        public WishOriginalDataViewModel(UserData userData)
        {
            WishTypeList = new List<string> { "---", "新手祈愿", "常驻祈愿", "角色活动祈愿", "武器活动祈愿" };
            ItemTypeList = new List<string> { "-", "角色", "武器" };
            ItemRankList = new List<string> { "-", "3", "4", "5" };
            var eventlist = KeqingNiuza.Core.Wish.Const.WishEventList;
            var zeroWishEvent = new WishEvent
            {
                Name = "---",
                StartTime = new DateTime(2020, 9, 15, 0, 0, 0, DateTimeKind.Local),
                EndTime = DateTime.Now,
                UpStar5 = new List<string>(),
                UpStar4 = new List<string>()
            };
            WishEventList = eventlist.Prepend(zeroWishEvent).ToList();
            SelectedWishEvent = WishEventList[0];

            WishDataList = LocalWishLogLoader.Load(userData.WishLogFile);
            WishDataList.Reverse();
            FilteredWishData = WishDataList;
        }

        public WishOriginalDataViewModel()
        {
            WishTypeList = new List<string> { "---", "新手祈愿", "常驻祈愿", "角色活动祈愿", "武器活动祈愿" };
            ItemTypeList = new List<string> { "-", "角色", "武器" };
            ItemRankList = new List<string> { "-", "3", "4", "5" };
            var eventlist = KeqingNiuza.Core.Wish.Const.WishEventList;
            var zeroWishEvent = new WishEvent
            {
                Name = "---",
                StartTime = new DateTime(2020, 9, 15, 0, 0, 0, DateTimeKind.Local),
                EndTime = DateTime.Now,
                UpStar5 = new List<string>(),
                UpStar4 = new List<string>()
            };
            WishEventList = eventlist.Prepend(zeroWishEvent).ToList();
            SelectedWishEvent = WishEventList[0];

            WishDataList = MainWindowViewModel.WishDataList.ToList();
            WishDataList.Reverse();
            FilteredWishData = WishDataList;
        }



        public void ResetFilter()
        {
            SelectedWishType = "---";
            SelectedItemType = "-";
            SelectedItemRank = "-";
            SelectedWishEvent = WishEventList[0];
            ToggleButton_Search_IsChecked = false;
            TextBox_Search_Text = "";
            FilteredWishData = WishDataList;
        }

        public void FilterWishData(bool delay = false)
        {
            if (delay)
            {
                //防止搜索框升起时，完成数据过滤卡住UI线程
                //现在搜索框没动画了
                //Thread.Sleep(160);
            }
            FilteredWishData = null;
            var tmp = WishDataList;
            switch (SelectedWishType)
            {
                case "新手祈愿":
                    tmp = tmp.FindAll(x => x.WishType == WishType.Novice);
                    break;
                case "常驻祈愿":
                    tmp = tmp.FindAll(x => x.WishType == WishType.Permanent);
                    break;
                case "角色活动祈愿":
                    tmp = tmp.FindAll(x => x.WishType == WishType.CharacterEvent || x.WishType == WishType.CharacterEvent_2);
                    break;
                case "武器活动祈愿":
                    tmp = tmp.FindAll(x => x.WishType == WishType.WeaponEvent);
                    break;
            }
            if (SelectedItemType != "-")
            {
                tmp = tmp.FindAll(x => x.ItemType == SelectedItemType);
            }
            if (SelectedItemRank != "-")
            {
                tmp = tmp.FindAll(x => x.Rank == int.Parse(SelectedItemRank));
            }
            if (SelectedWishEvent.Name != "---")
            {
                if (SelectedWishEvent.WishType == WishType.CharacterEvent)
                {
                    tmp = tmp.FindAll(x => (x.WishType == WishType.CharacterEvent || x.WishType == WishType.CharacterEvent_2) && x.Time > SelectedWishEvent.StartTime && x.Time < SelectedWishEvent.EndTime);
                }
                else
                {
                    tmp = tmp.FindAll(x => x.WishType == SelectedWishEvent.WishType && x.Time > SelectedWishEvent.StartTime && x.Time < SelectedWishEvent.EndTime);
                }
            }
            if (ToggleButton_Search_IsChecked && TextBox_Search_Text != "")
            {
                tmp = tmp.FindAll(x => x.Name.Contains(TextBox_Search_Text));
            }
            FilteredWishData = tmp;
        }

    }
}
