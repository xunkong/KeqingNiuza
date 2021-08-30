using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using HandyControl.Controls;
using KeqingNiuza.Service;

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

        private Updater Updater;


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

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Updater = new Updater();
                Updater.DownloadStarted += Updater_DownloadStarted;
                Updater.OneFileDownloaded += Updater_OneFileDownloaded;
                Updater.DownloadFinished += Updater_DownloadFinished;
                var result = await Updater.UpdateResourceFiles();
            }
            catch (Exception ex)
            {
                DownloadState = "出了一些问题";
                Growl.Warning(ex.Message);
                Log.OutputLog(LogType.Warning, "First download resource", ex);
            }
            LoadingCircle_Download.Visibility = Visibility.Hidden;
        }

        private void Updater_DownloadFinished(object sender, bool e)
        {
            if (e)
            {
                DownloadState = "出了一些问题";
            }
            else
            {
                DownloadState = "下载完成";
            }
        }

        private void Updater_DownloadStarted(object sender, EventArgs e)
        {
            DownloadSize = (double)Updater.DownloadSize / 1024 / 1024;
            FileCount = Updater.AllFilesCount;
            DownloadState = "正在下载";
        }


        private void Updater_OneFileDownloaded(object sender, EventArgs e)
        {
            HasDownload = Updater.DownloadedFilesCount;
        }


    }
}
