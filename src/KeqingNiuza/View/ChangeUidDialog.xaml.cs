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
using KeqingNiuza.Model;

namespace KeqingNiuza.View
{
    /// <summary>
    /// ChangeUidDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeUidDialog : UserControl, IDialogResultable<UserData>
    {
        public ChangeUidDialog(List<UserData> userDataList)
        {
            InitializeComponent();
            UserDataList = userDataList;
            DataContext = this;
        }



        public UserData Result { get; set; }
        public Action CloseAction { get; set; }

        public List<UserData> UserDataList { get; set; }

        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            Result = ListView_UserData.SelectedItem as UserData;
            ControlCommands.Close.Execute(null, this);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            ControlCommands.Close.Execute(null, this);
        }


    }
}
