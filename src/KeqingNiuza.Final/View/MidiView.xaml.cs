using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Controls;
using KeqingNiuza.Core.Midi;
using KeqingNiuza.ViewModel;

namespace KeqingNiuza.View
{
    /// <summary>
    /// MidiView.xaml 的交互逻辑
    /// </summary>
    public partial class MidiView : UserControl
    {
        public MidiView()
        {
            InitializeComponent();
        }

        public MidiViewModel ViewModel { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel == null)
                {
                    ViewModel = new MidiViewModel();
                    DataContext = ViewModel;
                }
            }
            catch (Exception ex)
            {
                Growl.Warning(ex.Message);
            }
        }

        private void ListBox_MidiFileInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ListBox_MidiFileInfo.SelectedItem as MidiFileInfo;
            ViewModel?.ChangePlayFile(item);
        }

        private void Button_Restart_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.RestartOrRefresh();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.PlayLast();
        }

        private void Button_Forward_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.PlayNext();
        }

        private void ToggleButton_CheckTrack_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.ChangeMidiTrack();
        }
    }
}
