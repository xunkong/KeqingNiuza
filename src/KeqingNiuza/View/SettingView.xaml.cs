using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using HandyControl.Controls;
using KeqingNiuza.Service;
using Microsoft.AppCenter;

namespace KeqingNiuza.View
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public SettingView()
        {
            InitializeComponent();
            GuidText.Text = AppCenter.GetInstallIdAsync().Result.ToString();
        }

        public bool IsAdmin => ScheduleTask.IsAdmin();


        public bool IsAutoUpdate
        {
            get => Properties.Settings.Default.IsAutoUpdate;
            set
            {
                Properties.Settings.Default.IsAutoUpdate = value;
                OnPropertyChanged();
            }
        }

        public bool IsUpdateShowResult
        {
            get => Properties.Settings.Default.IsUpdateShowResult;
            set
            {
                Properties.Settings.Default.IsUpdateShowResult = value;
                OnPropertyChanged();
            }
        }

        public bool IsUpdateRestart
        {
            get => Properties.Settings.Default.IsUpdateRestart;
            set
            {
                Properties.Settings.Default.IsUpdateRestart = value;
                OnPropertyChanged();
            }
        }

        public bool IsLogonTrigger
        {
            get => Properties.Settings.Default.IsLogonTrigger;
            set
            {
                if (ChangeScheduleTask(value, IsGenshinStartTrigger))
                {
                    Properties.Settings.Default.IsLogonTrigger = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsGenshinStartTrigger
        {
            get => Properties.Settings.Default.IsGenshinStartTrigger;
            set
            {
                if (ChangeScheduleTask(IsLogonTrigger, value))
                {
                    Properties.Settings.Default.IsGenshinStartTrigger = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsOversea
        {
            get => Properties.Settings.Default.IsOversea;
            set
            {
                Properties.Settings.Default.IsOversea = value;
                OnPropertyChanged();
            }
        }



        private bool _DailyCheck_IsAutoCheckIn = Properties.Settings.Default.DailyCheck_IsAutoCheckIn;
        public bool DailyCheck_IsAutoCheckIn
        {
            get { return _DailyCheck_IsAutoCheckIn; }
            set
            {
                _DailyCheck_IsAutoCheckIn = value;
                OnPropertyChanged();
            }
        }


        private DateTime _DailyCheck_StartTime = Properties.Settings.Default.DailyCheck_StartTime;
        public DateTime DailyCheck_StartTime
        {
            get { return _DailyCheck_StartTime; }
            set
            {
                _DailyCheck_StartTime = value;
                OnPropertyChanged();
            }
        }


        private TimeSpan _DailyCheck_RandomDelay = Properties.Settings.Default.DailyCheck_RandomDelay;
        public TimeSpan DailyCheck_RandomDelay
        {
            get { return _DailyCheck_RandomDelay; }
            set
            {
                _DailyCheck_RandomDelay = value;
                OnPropertyChanged();
            }
        }



        private bool _DialyCheck_AlwaysShowResult = Properties.Settings.Default.DialyCheck_AlwaysShowResult;
        public bool DialyCheck_AlwaysShowResult
        {
            get { return _DialyCheck_AlwaysShowResult; }
            set
            {
                _DialyCheck_AlwaysShowResult = value;
                OnPropertyChanged();
            }
        }



        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }



        private bool ChangeScheduleTask(bool logon, bool genshinstart)
        {
            try
            {
                TaskTrigger trigger = TaskTrigger.None;
                if (logon)
                {
                    trigger = trigger | TaskTrigger.Logon;
                }
                if (genshinstart)
                {
                    trigger = trigger | TaskTrigger.GenshinStart;
                }
                ScheduleTask.AddTask(trigger);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Growl.Warning("权限不足");
                return false;
            }
            catch (Exception ex)
            {
                Growl.Warning(ex.Message);
                Log.OutputLog(LogType.Warning, "ChangeScheduleTask", ex);
                return false;
            }
        }

        private void GuidText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(GuidText.Text.ToString());
            Growl.Success("复制成功");
        }

        private void Button_DailyCheckSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DailyCheckTask.AddTask(DailyCheck_IsAutoCheckIn, DailyCheck_StartTime, DailyCheck_RandomDelay);
                Properties.Settings.Default.DailyCheck_IsAutoCheckIn = DailyCheck_IsAutoCheckIn;
                Properties.Settings.Default.DailyCheck_StartTime = DailyCheck_StartTime;
                Properties.Settings.Default.DailyCheck_RandomDelay = DailyCheck_RandomDelay;
                Properties.Settings.Default.DialyCheck_AlwaysShowResult = DialyCheck_AlwaysShowResult;
                Growl.Success("保存成功");
            }
            catch (Exception ex)
            {
                Growl.Warning(ex.Message);
                Log.OutputLog(LogType.Warning, "DailyCheckSettingSave", ex);
            }
        }
    }
}
