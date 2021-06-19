using HandyControl.Controls;
using KeqingNiuza.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                return false;
            }
        }


    }
}
