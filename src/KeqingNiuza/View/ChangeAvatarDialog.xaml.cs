using KeqingNiuza.Common;
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
using HandyControl.Tools.Extension;
using HandyControl.Interactivity;
using System.IO;

namespace KeqingNiuza.View
{
    /// <summary>
    /// ChangeAvatarDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeAvatarDialog : UserControl, IDialogResultable<string>
    {
        public ChangeAvatarDialog()
        {
            InitializeComponent();
            DataContext = this;
            AvatarList = Directory.GetFiles("resource\\avatar").ToList();
        }

        public List<string> AvatarList { get; set; }
        public string Result { get; set; }
        public Action CloseAction { get; set; }

        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            Result = ListBox_Avatar.SelectedItem as string;
            ControlCommands.Close.Execute(null, this);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            ControlCommands.Close.Execute(null, this);
        }

        private void ListBox_Avatar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Result = ListBox_Avatar.SelectedItem as string;
            ControlCommands.Close.Execute(null, this);
        }
    }
}
