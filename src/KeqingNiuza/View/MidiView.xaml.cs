using KeqingNiuza.Midi;
using KeqingNiuza.Model;
using KeqingNiuza.Service;
using KeqingNiuza.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (ViewModel == null)
            {
                ViewModel = new MidiViewModel();
                DataContext = ViewModel;
            }
        }

        private void ListBox_MidiFileInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ListBox_MidiFileInfo.SelectedItem as MidiFileInfo;
            ViewModel.ChangePlayFile(item);
        }

        private void Button_Restart_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RestartOrRefresh();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PlayLast();
        }

        private void Button_Forward_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PlayNext();
        }

    }
}
