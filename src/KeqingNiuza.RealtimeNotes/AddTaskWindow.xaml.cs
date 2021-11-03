using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using KeqingNiuza.RealtimeNotes.Services;

namespace KeqingNiuza.RealtimeNotes
{
    /// <summary>
    /// AddTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddTaskWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddTaskWindow()
        {
            InitializeComponent();
        }



        private int _Interval = 15;
        public int Interval
        {
            get { return _Interval; }
            set
            {
                _Interval = value;
                OnPropertyChanged();
            }
        }


        private string _StateText;
        public string StateText
        {
            get { return _StateText; }
            set
            {
                _StateText = value;
                OnPropertyChanged();
            }
        }


        private void Button_AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (Interval <= 0)
            {
                StateText = "间隔需要≥1分钟";
                return;
            }
            try
            {
                BackgroundService.AddScheduleTask(Interval);
                StateText = "添加成功";
            }
            catch (Exception ex)
            {
                StateText = $"添加失败：{ex.Message}";
                LogService.Log(ex.ToString());
            }
        }
    }
}
