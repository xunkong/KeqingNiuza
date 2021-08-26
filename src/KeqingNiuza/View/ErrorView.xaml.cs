using System;
using System.Windows;
using System.Windows.Controls;

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
