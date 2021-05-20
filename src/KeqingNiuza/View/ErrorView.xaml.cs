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
    /// ErrorView.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorView : UserControl
    {
        public ErrorView(Exception ex)
        {
            InitializeComponent();
            TextBlock_ErrorMessage.Text = GetInnerMessage(ex);
            Ex = ex;
        }

        private readonly Exception Ex;

        private void Button_Detail_Click(object sender, RoutedEventArgs e)
        {
            TextBlock_Detail.Text = Ex.ToString();
        }

        private string GetInnerMessage(Exception ex)
        {
            string mesage = ex.Message;
            Exception innerEx = ex;
            while (innerEx.InnerException != null)
            {
                innerEx = innerEx.InnerException;
                mesage = innerEx.Message;
            }
            return mesage;
        }
    }
}
