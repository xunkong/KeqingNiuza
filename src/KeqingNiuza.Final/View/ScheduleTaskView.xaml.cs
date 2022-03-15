using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using KeqingNiuza.Model;
using KeqingNiuza.Service;

namespace KeqingNiuza.View
{
    /// <summary>
    /// ScheduleTaskView.xaml 的交互逻辑
    /// </summary>
    public partial class ScheduleTaskView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Timer timer;

        public ScheduleTaskView()
        {
            InitializeComponent();
            if (File.Exists($"{Const.UserDataPath}\\ScheduleTask.json"))
            {
                var json = File.ReadAllText($"{Const.UserDataPath}\\ScheduleTask.json");
                ScheduleTaskList = JsonSerializer.Deserialize<ObservableCollection<ScheduleInfo>>(json, Const.JsonOptions);
            }
            else
            {
                ScheduleTaskList = new ObservableCollection<ScheduleInfo>();
            }
            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var item in ScheduleTaskList)
            {
                item.Refresh();
            }
        }


        private ObservableCollection<ScheduleInfo> _ScheduleTaskList;
        public ObservableCollection<ScheduleInfo> ScheduleTaskList
        {
            get { return _ScheduleTaskList; }
            set
            {
                _ScheduleTaskList = value;
                OnPropertyChanged();
            }
        }

        private ScheduleInfo _SelectedScheduleInfo;
        public ScheduleInfo SelectedScheduleInfo
        {
            get { return _SelectedScheduleInfo; }
            set
            {
                _SelectedScheduleInfo = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedInfoRemainingTime");
                OnPropertyChanged("SelectedInfoNextTriggerTime");
                OnPropertyChanged("SelectedInfoCurrentValue");
                OnPropertyChanged("SelectedInfoNextMaxValueTime");
            }
        }

        public TimeSpan? SelectedInfoRemainingTime
        {
            get
            {
                if (SelectedScheduleInfo != null)
                {
                    return SelectedScheduleInfo.RemainingTime;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (SelectedScheduleInfo != null)
                {
                    SelectedScheduleInfo.RemainingTime = (TimeSpan)value;
                    OnPropertyChanged();
                    OnPropertyChanged("SelectedInfoRemainingTime");
                    OnPropertyChanged("SelectedInfoNextTriggerTime");
                    OnPropertyChanged("SelectedInfoCurrentValue");
                    OnPropertyChanged("SelectedInfoNextMaxValueTime");
                }
            }
        }

        public DateTime? SelectedInfoNextTriggerTime
        {
            get
            {
                if (SelectedScheduleInfo != null)
                {
                    return SelectedScheduleInfo.NextTriggerTime;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (SelectedScheduleInfo != null)
                {
                    SelectedScheduleInfo.NextTriggerTime = (DateTime)value;
                    OnPropertyChanged();
                    OnPropertyChanged("SelectedInfoRemainingTime");
                    OnPropertyChanged("SelectedInfoNextTriggerTime");
                    OnPropertyChanged("SelectedInfoCurrentValue");
                    OnPropertyChanged("SelectedInfoNextMaxValueTime");
                }
            }
        }

        public int? SelectedInfoCurrentValue
        {
            get
            {
                if (SelectedScheduleInfo != null)
                {
                    return SelectedScheduleInfo.CurrentValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (SelectedScheduleInfo != null)
                {
                    SelectedScheduleInfo.CurrentValue = (int)value;
                    OnPropertyChanged();
                    OnPropertyChanged("SelectedInfoRemainingTime");
                    OnPropertyChanged("SelectedInfoNextTriggerTime");
                    OnPropertyChanged("SelectedInfoCurrentValue");
                    OnPropertyChanged("SelectedInfoNextMaxValueTime");
                }
            }
        }

        public DateTime? SelectedInfoNextMaxValueTime
        {
            get
            {
                if (SelectedScheduleInfo != null)
                {
                    return SelectedScheduleInfo.NextMaxValueTime;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (SelectedScheduleInfo != null)
                {
                    SelectedScheduleInfo.NextMaxValueTime = (DateTime)value;
                    OnPropertyChanged();
                    OnPropertyChanged("SelectedInfoRemainingTime");
                    OnPropertyChanged("SelectedInfoNextTriggerTime");
                    OnPropertyChanged("SelectedInfoCurrentValue");
                    OnPropertyChanged("SelectedInfoNextMaxValueTime");
                }
            }
        }

        private void Save()
        {
            if (ScheduleTaskList.Count > 0)
            {
                var list = ScheduleTaskList.Where(x => !string.IsNullOrWhiteSpace(x.Name));
                var json = JsonSerializer.Serialize(list, Const.JsonOptions);
                File.WriteAllText($"{Const.UserDataPath}\\ScheduleTask.json", json);
                ScheduleTask.AddRealTimeNotifacation(ScheduleTaskList);
            }
        }

        private void ToggleButton_IsEnable_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            var context = (sender as Button).DataContext as ScheduleInfo;
            switch (context.TriggerType)
            {
                case ScheduleInfoTriggerType.None:
                    break;
                case ScheduleInfoTriggerType.Countdown:
                    SelectedInfoRemainingTime = context.Interval;
                    break;
                case ScheduleInfoTriggerType.Recovery:
                    SelectedInfoCurrentValue = 0;
                    break;
                case ScheduleInfoTriggerType.FixedTime:
                    var now = DateTime.Now;
                    var next = now.Date.AddDays(context.Custom_DelayDay) + context.Custom_TriggerTime.TimeOfDay;
                    context.Interval = next - now;
                    SelectedInfoRemainingTime = next - now;
                    break;
                default:
                    break;
            }
            Save();
        }

        private void Button_MoveBack_Click(object sender, RoutedEventArgs e)
        {
            var context = (sender as Button).DataContext as ScheduleInfo;
            var index = ScheduleTaskList.IndexOf(context);
            if (index != 0)
            {
                ScheduleTaskList.RemoveAt(index);
                ScheduleTaskList.Insert(index - 1, context);
                Save();
            }
        }

        private void Button_MoveNext_Click(object sender, RoutedEventArgs e)
        {
            var context = (sender as Button).DataContext as ScheduleInfo;
            var index = ScheduleTaskList.IndexOf(context);
            if (index != ScheduleTaskList.Count)
            {
                ScheduleTaskList.RemoveAt(index);
                if (index == ScheduleTaskList.Count)
                {
                    ScheduleTaskList.Add(context);
                }
                else
                {
                    ScheduleTaskList.Insert(index + 1, context);
                }
                Save();
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var context = (sender as Button).DataContext as ScheduleInfo;
            ScheduleTaskList.Remove(context);
            var index = ScheduleTaskList.IndexOf(context);
            Save();
        }

        private void RadioButton_Type_Click(object sender, RoutedEventArgs e)
        {
            //var tag = (sender as RadioButton).Tag as string;
            //if (tag == "Countdown" && SelectedScheduleInfo != null)
            //{
            //    SelectedScheduleInfo.IsCountdownType = true;
            //}
            //if (tag == "Replenish" && SelectedScheduleInfo != null)
            //{
            //    SelectedScheduleInfo.IsCountdownType = false;
            //}
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var newInfo = new ScheduleInfo();
            ScheduleTaskList.Add(newInfo);
            SelectedScheduleInfo = newInfo;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Button_TestNotifacation_Click(object sender, RoutedEventArgs e)
        {
            ScheduleTask.TestNotifacation();
        }

    }
}
