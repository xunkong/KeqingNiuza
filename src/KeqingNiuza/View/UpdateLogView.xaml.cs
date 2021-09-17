using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace KeqingNiuza.View
{
    /// <summary>
    /// UpdateLogView.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateLogView : UserControl
    {
        public UpdateLogView()
        {
            InitializeComponent();
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }


    }
}
