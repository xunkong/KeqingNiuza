using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KeqingNiuza.Core.Wish;
using KeqingNiuza.ViewModel;

namespace KeqingNiuza.View
{
    /// <summary>
    /// WishItemDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class WishItemDetailView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler BackEvent;

        private List<WishEvent> WishEventList = Const.WishEventList;

        public WishItemDetailView(List<ItemInfo> list)
        {
            InitializeComponent();
            ItemInfoList = list.OrderByDescending(x => x.Rank).ThenByDescending(x => x.Count).ThenByDescending(x => x.LastGetTime).ToList();
            ItemsControl_ItemList.ItemsSource = ItemInfoList;
            SelectedItemInfo = ItemInfoList.First();
        }

        public WishItemDetailView(List<ItemInfo> list, ItemInfo selectedInfo)
        {
            InitializeComponent();
            ItemInfoList = list.OrderByDescending(x => x.Rank).ThenByDescending(x => x.Count).ThenByDescending(x => x.LastGetTime).ToList();
            ItemsControl_ItemList.ItemsSource = ItemInfoList;
            SelectedItemInfo = selectedInfo;
        }


        private List<ItemInfo> _ItemInfoList;
        public List<ItemInfo> ItemInfoList
        {
            get { return _ItemInfoList; }
            set
            {
                _ItemInfoList = value;
                OnPropertyChanged();
            }
        }


        private ItemInfo _SelectedItemInfo;
        public ItemInfo SelectedItemInfo
        {
            get { return _SelectedItemInfo; }
            set
            {
                _SelectedItemInfo = value;
                SelectedItemInfo_Changed(value);
                OnPropertyChanged();
            }
        }

        private int _Count_XiaoBaoDi;
        /// <summary>
        /// 小保底次数
        /// </summary>
        public int Count_XiaoBaoDi
        {
            get { return _Count_XiaoBaoDi; }
            set
            {
                _Count_XiaoBaoDi = value;
                OnPropertyChanged();
            }
        }

        private int _Count_DaBaoDi;
        /// <summary>
        /// 大保底次数
        /// </summary>
        public int Count_DaBaoDi
        {
            get { return _Count_DaBaoDi; }
            set
            {
                _Count_DaBaoDi = value;
                OnPropertyChanged();
            }
        }

        private int _Count_BaoDiNei;
        /// <summary>
        /// 常驻池保底次数
        /// </summary>
        public int Count_BaoDiNei
        {
            get { return _Count_BaoDiNei; }
            set
            {
                _Count_BaoDiNei = value;
                OnPropertyChanged();
            }
        }

        private int _Count_Up;
        /// <summary>
        /// 在Up池中获取的次数
        /// </summary>
        public int Count_Up
        {
            get { return _Count_Up; }
            set
            {
                _Count_Up = value;
                OnPropertyChanged();
            }
        }


        private int _Count_NotUp;
        /// <summary>
        /// 在非Up池中获取的次数
        /// </summary>
        public int Count_NotUp
        {
            get { return _Count_NotUp; }
            set
            {
                _Count_NotUp = value;
                OnPropertyChanged();
            }
        }


        private DateTime _StartTime;
        /// <summary>
        /// 第一次获取时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _StartTime; }
            set
            {
                _StartTime = value;
                OnPropertyChanged();
            }
        }


        private DateTime _EndTime;
        /// <summary>
        /// 最后一次获取时间
        /// </summary>
        public DateTime EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
                OnPropertyChanged();
            }
        }


        private int _Count_Character;
        /// <summary>
        /// 角色池中获取的次数
        /// </summary>
        public int Count_Character
        {
            get { return _Count_Character; }
            set
            {
                _Count_Character = value;
                OnPropertyChanged();
            }
        }


        private int _Count_Weapon;
        /// <summary>
        /// 武器池中获取的次数
        /// </summary>
        public int Count_Weapon
        {
            get { return _Count_Weapon; }
            set
            {
                _Count_Weapon = value;
                OnPropertyChanged();
            }
        }

        private int _Count_Permanent;
        /// <summary>
        /// 常驻池中获取的次数
        /// </summary>
        public int Count_Permanent
        {
            get { return _Count_Permanent; }
            set
            {
                _Count_Permanent = value;
                OnPropertyChanged();
            }
        }

        private int _Count_Novice;
        /// <summary>
        /// 新手池中获取的次数
        /// </summary>
        public int Count_Novice
        {
            get { return _Count_Novice; }
            set
            {
                _Count_Novice = value;
                OnPropertyChanged();
            }
        }


        private List<WishData> _SourceList;
        public List<WishData> SourceList
        {
            get { return _SourceList; }
            set
            {
                _SourceList = value;
                OnPropertyChanged();
            }
        }


        private List<RelativeEvent> _RelativeEventList;
        public List<RelativeEvent> RelativeEventList
        {
            get { return _RelativeEventList; }
            set
            {
                _RelativeEventList = value;
                OnPropertyChanged();
            }
        }


        private void SelectedItemInfo_Changed(ItemInfo info)
        {
            if (info == null)
            {
                return;
            }
            Count_XiaoBaoDi = info.WishDataList.Count(x => x.GuaranteeType == "小保底");
            Count_DaBaoDi = info.WishDataList.Count(x => x.GuaranteeType == "大保底");
            Count_BaoDiNei = info.WishDataList.Count(x => x.GuaranteeType == "保底内");
            Count_Up = info.WishDataList.Count(x => x.IsUp == true);
            Count_NotUp = info.WishDataList.Count(x => x.IsUp == false);
            StartTime = info.WishDataList.First().Time;
            EndTime = info.WishDataList.Last().Time;
            Count_Character = info.WishDataList.Count(x => x.WishType == WishType.CharacterEvent);
            Count_Weapon = info.WishDataList.Count(x => x.WishType == WishType.WeaponEvent);
            Count_Permanent = info.WishDataList.Count(x => x.WishType == WishType.Permanent);
            Count_Novice = info.WishDataList.Count(x => x.WishType == WishType.Novice);
            SourceList = info.WishDataList.Where(x => x.WishType == WishType.CharacterEvent).ToList();
            RadioButton_Character.IsChecked = true;

            var eventList = new List<RelativeEvent>();
            var infoList = info.WishDataList.Where(x => x.WishType == WishType.Novice).ToList();
            if (infoList.Any())
            {
                var allList = MainWindowViewModel.WishDataList.Where(x => x.WishType == WishType.Novice).ToList();
                var relativeEvent = new RelativeEvent
                {
                    EventName = "新手池",
                    TotalCount = allList.Count,
                    ThisCount = infoList.Count,
                    Count_XiaoBaoDi = infoList.Count(x => x.GuaranteeType == "保底内"),
                };
                eventList.Add(relativeEvent);
            }
            foreach (var @event in WishEventList)
            {
                var infolist = info.WishDataList.Where(x => x.WishType == @event.WishType && x.Time >= @event.StartTime && x.Time <= @event.EndTime).ToList();
                if (infolist.Any())
                {
                    var allList = MainWindowViewModel.WishDataList.Where(x => x.WishType == @event.WishType && x.Time >= @event.StartTime && x.Time <= @event.EndTime).ToList();
                    var relativeEvent = new RelativeEvent
                    {
                        EventName = @event.DisplayName,
                        StartTime = @event.StartTime,
                        EndTime = @event.EndTime,
                        UpItems = @event.UpItems,
                        WishEvent = @event,
                        TotalCount = allList.Count,
                        ThisCount = infolist.Count,
                        Count_XiaoBaoDi = infolist.Count(x => x.GuaranteeType == "小保底"),
                        Count_DaBaoDi = infolist.Count(x => x.GuaranteeType == "大保底"),
                    };
                    eventList.Add(relativeEvent);
                }
            }
            infoList = info.WishDataList.Where(x => x.WishType == WishType.Permanent).ToList();
            if (infoList.Any())
            {
                var allList = MainWindowViewModel.WishDataList.Where(x => x.WishType == WishType.Permanent).ToList();
                var relativeEvent = new RelativeEvent
                {
                    EventName = $"常驻池",
                    TotalCount = allList.Count,
                    ThisCount = infoList.Count,
                    Count_XiaoBaoDi = infoList.Count(x => x.GuaranteeType == "保底内"),
                };
                eventList.Add(relativeEvent);
            }
            RelativeEventList = eventList.Reverse<RelativeEvent>().ToList();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(this, null);
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            SelectedItemInfo = grid.DataContext as ItemInfo;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as RadioButton).Tag as string;
            switch (content)
            {
                case "Character":
                    SourceList = SelectedItemInfo.WishDataList.Where(x => x.WishType == WishType.CharacterEvent).ToList();
                    break;
                case "Weapon":
                    SourceList = SelectedItemInfo.WishDataList.Where(x => x.WishType == WishType.WeaponEvent).ToList();
                    break;
                case "Permanent":
                    SourceList = SelectedItemInfo.WishDataList.Where(x => x.WishType == WishType.Permanent).ToList();
                    break;
                case "Novice":
                    SourceList = SelectedItemInfo.WishDataList.Where(x => x.WishType == WishType.Novice).ToList();
                    break;
                default:
                    break;
            }
        }
    }
}
