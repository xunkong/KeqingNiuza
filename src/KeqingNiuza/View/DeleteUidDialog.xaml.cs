using HandyControl.Interactivity;
using HandyControl.Tools.Extension;
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
    /// DeleteUidDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DeleteUidDialog : UserControl, IDialogResultable<bool>
    {
        public DeleteUidDialog()
        {
            InitializeComponent();
        }

        public DeleteUidDialog(int uid)
        {
            InitializeComponent();
            TextBlock_Info.Text = $"删除Uid:{uid}的所有数据？";
        }


        public bool Result { get; set; }
        public Action CloseAction { get; set; }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            ControlCommands.Close.Execute(null, this);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            ControlCommands.Close.Execute(null, this);
        }
    }
}
