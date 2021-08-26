using KeqingNiuza.Common;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
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
            if (File.Exists("UserData\\ScheduleTask.json"))
            {
                var json = File.ReadAllText("UserData\\ScheduleTask.json");
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
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
                    OnPropertyChanged("SelectedInfoNextTriggerTime");
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
                    OnPropertyChanged("SelectedInfoRemainingTime");
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
                    OnPropertyChanged("SelectedInfoNextMaxValueTime");
                    OnPropertyChanged();
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
                    OnPropertyChanged("SelectedInfoCurrentValue");
                }
            }
        }

        private void Save()
        {
            if (ScheduleTaskList.Count > 0)
            {
                var list = ScheduleTaskList.Where(x => !string.IsNullOrWhiteSpace(x.Name));
                var json = JsonSerializer.Serialize(list, Const.JsonOptions);
                File.WriteAllText("UserData\\ScheduleTask.json", json);
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
            context.RemainingTime = context.Interval;
            context.CurrentValue = 0;
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
            var tag = (sender as RadioButton).Tag as string;
            if (tag == "Countdown" && SelectedScheduleInfo != null)
            {
                SelectedScheduleInfo.IsCountdownType = true;
            }
            if (tag == "Replenish" && SelectedScheduleInfo != null)
            {
                SelectedScheduleInfo.IsCountdownType = false;
            }
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
