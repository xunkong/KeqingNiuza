using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace KeqingNiuza.View
{
    /// <summary>
    /// WelcomeView.xaml 的交互逻辑
    /// </summary>
    public partial class WelcomeView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public WelcomeView()
        {
            InitializeComponent();
        }


        private int _FileCount;
        public int FileCount
        {
            get { return _FileCount; }
            set
            {
                _FileCount = value;
                OnPropertyChanged();
            }
        }

        private int _HasDownload;
        public int HasDownload
        {
            get { return _HasDownload; }
            set
            {
                _HasDownload = value;
                OnPropertyChanged();
            }
        }

        private double _DownloadSize;
        public double DownloadSize
        {
            get { return _DownloadSize; }
            set
            {
                _DownloadSize = value;
                OnPropertyChanged();
            }
        }

        private string _DownloadState;
        public string DownloadState
        {
            get { return _DownloadState; }
            set
            {
                _DownloadState = value;
                OnPropertyChanged();
            }
        }



        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }



    }
}
