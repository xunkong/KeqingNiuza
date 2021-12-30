using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KeqingNiuza.Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow(string arg = null)
        {
            _arg = arg;
            Initialized += Window_Initialized;
            Loaded += Window_Loaded;
            InitializeComponent();
            VersionText.Text = MetaData.FileVersion;
        }



        private readonly string versionUrl = "https://xw6dp97kei-1306705684.file.myqcloud.com/keqingniuza/meta/version.json";

        private readonly string wallpaperUrl = "https://xw6dp97kei-1306705684.file.myqcloud.com/keqingniuza/meta/wallpaper.json";

        private readonly string geoIpApi = "http://ip-api.com/json";

        private string _arg;

        private Downloader _downloader;

        private bool _canceled;


        private string _InfoTest;
        public string InfoTest
        {
            get { return _InfoTest; }
            set
            {
                _InfoTest = value;
                OnPropertyChanged();
            }
        }


        private string _ProgressTest;
        public string ProgressTest
        {
            get { return _ProgressTest; }
            set
            {
                _ProgressTest = value;
                OnPropertyChanged();
            }
        }


        private string _SpeedTest;
        public string SpeedTest
        {
            get { return _SpeedTest; }
            set
            {
                _SpeedTest = value;
                OnPropertyChanged();
            }
        }


        private bool _CanCancel = true;
        public bool CanCancel
        {
            get { return _CanCancel; }
            set
            {
                _CanCancel = value;
                OnPropertyChanged();
            }
        }


        private bool _CanRefresh;
        public bool CanRefresh
        {
            get { return _CanRefresh; }
            set
            {
                _CanRefresh = value;
                OnPropertyChanged();
            }
        }



        private void Window_Initialized(object sender, EventArgs e)
        {
            BitmapImage bitmap = null;
            WallpperInfoText.Text = "Sevenix2 - Moon over Monstadt";
            try
            {
                string[] files = Directory.GetFiles(".\\wallpaper");
                if (files.Any())
                {
                    Random random = new Random((int)DateTime.Now.Ticks);
                    // 文件越靠后，被选中的概率越大
                    var index = (int)(files.Length * Math.Log(1 + (Math.E - 1) * random.NextDouble()));
                    Console.WriteLine(files.Length + " " + index);
                    string file = files[index];
                    using (var s = File.OpenRead(file))
                    {
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = null;
                        bitmap.StreamSource = s;
                        bitmap.EndInit();
                    }
                    var name = Path.GetFileNameWithoutExtension(file);
                    var t1 = Regex.Match(name, @"\[([^\]]+)\]").Groups[1].Value;
                    var t2 = Regex.Match(name, @"\]([^\[]+)").Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(t1) && !string.IsNullOrWhiteSpace(t2))
                    {
                        WallpperInfoText.Text = $"{t1.Trim()} - {t2.Trim()}";
                    }
                    Width = Height * bitmap.PixelWidth / bitmap.PixelHeight;
                }
                else
                {
                    bitmap = new BitmapImage(new Uri("./Moon over Monstadt.jpg", UriKind.Relative));
                }
            }
            catch (Exception)
            {
                bitmap = new BitmapImage(new Uri("./Moon over Monstadt.jpg", UriKind.Relative));
            }
            finally
            {
                SplashImage.Source = bitmap;
            }
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_WindowMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_WindowClose_Click(object sender, RoutedEventArgs e)
        {
            _downloader?.Cancel();
            if (Directory.Exists(".\\deleting"))
            {
                var files = Directory.GetFiles(".\\deleting");
                foreach (var file in files)
                {
                    var dest = file.Replace("\\deleting", "");
                    if (!File.Exists(dest))
                    {
                        File.Move(file, dest);
                    }
                }
            }
            Close();
        }


        private void Button_Skip_Click(object sender, RoutedEventArgs e)
        {
            CancelAndSkip();
        }


        private async void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (CanRefresh)
            {
                CanRefresh = false;
                if (_arg == "--download-wallpaper")
                {
                    await DownloadWallpaper();
                    return;
                }
                await UpdateTask();
            }
        }


        private void Window_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                CancelAndSkip();
            }
            if (e.RightButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                Close();
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            if (e.Key == Key.Space || e.Key == Key.Enter)
            {
                CancelAndSkip();
            }
        }


        private void CancelAndSkip()
        {
            if (CanCancel)
            {
                _canceled = true;
                if (_arg == "--download-wallpaper")
                {
                    Close();
                    return;
                }
                Process.Start(".\\bin\\KeqingNiuza.exe");
                Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs _)
        {
            try
            {
                if (Directory.Exists(".\\splash"))
                {
                    Directory.Delete(".\\splash", true);
                }
            }
            catch { }
            if (_arg == "--download-wallpaper")
            {
                await DownloadWallpaper();
                return;
            }
            await UpdateTask();
        }


        private async Task DownloadWallpaper()
        {
            InfoTest = "正在检查更新";
            await Task.Delay(100);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", $"KeqingLauncher/{MetaData.FileVersion} UserId/{MetaData.UserId}");
            string json = null;
            try
            {
                json = await client.GetStringAsync(wallpaperUrl);

            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                if (_canceled)
                {
                    return;
                }
                try
                {
                    Util.OutputLog(ex);
                }
                catch { }
                InfoTest = ex.Message;
                CanRefresh = true;
                return;
            }
            var remoteList = JsonConvert.DeserializeObject<List<KeqingNiuzaFileInfo>>(json);
            remoteList.ForEach(x => x.Path = Path.GetFullPath(x.Path));
            Directory.CreateDirectory(".\\wallpaper");
            var fs = Directory.GetFiles(".\\wallpaper", "*");
            var localFiles = await Task.Run(() => fs.Select(x => new KeqingNiuzaFileInfo(x)).ToList());
            var downloadingFiles = remoteList.Except(localFiles).ToList();
            await Task.Delay(100);
            if (_canceled)
            {
                return;
            }
            if (downloadingFiles?.Any() ?? false)
            {
                try
                {
                    var ipjson = await client.GetStringAsync(geoIpApi);
                    var jobj = JObject.Parse(ipjson);
                    if (jobj["countryCode"].Value<string>() == "CN")
                    {
                        foreach (var item in downloadingFiles)
                        {
                            item.Url = item.Url_CN;
                        }
                    }
                    else
                    {
                        foreach (var item in downloadingFiles)
                        {
                            item.Url = item.Url_OS;
                        }
                    }
                }
                catch { }
                CanCancel = false;
                try
                {
                    await DownloadFiles(downloadingFiles);
                }
                catch (HttpRequestException ex)
                {
                    InfoTest = ex.Message;
                    CanRefresh = true;
                    CanCancel = true;
                    return;
                }
            }
            else
            {
                InfoTest = "已下载所有推荐壁纸";
                await Task.Delay(1000);
            }
            Close();
        }


        private async Task UpdateTask()
        {
            InfoTest = "正在检查更新";
            await Task.Delay(100);
            List<KeqingNiuzaFileInfo> list = null;
            try
            {
                list = await TestUpdate();
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                if (_canceled)
                {
                    return;
                }
                try
                {
                    Util.OutputLog(ex);
                }
                catch { }
                InfoTest = ex.Message;
                CanRefresh = true;
                return;
            }
            await Task.Delay(100);
            if (_canceled)
            {
                return;
            }
            if (list?.Any() ?? false)
            {
                try
                {
                    var client = new HttpClient();
                    var json = await client.GetStringAsync(geoIpApi);
                    var jobj = JObject.Parse(json);
                    if (jobj["countryCode"].Value<string>() == "CN")
                    {
                        foreach (var item in list)
                        {
                            item.Url = item.Url_CN;
                        }
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            item.Url = item.Url_OS;
                        }
                    }
                }
                catch { }
                CanCancel = false;
                try
                {
                    await DownloadFiles(list);
                }
                catch (HttpRequestException ex)
                {
                    InfoTest = ex.Message;
                    CanRefresh = true;
                    CanCancel = true;
                    return;
                }
            }
            Process.Start(".\\bin\\KeqingNiuza.exe");
            Close();
        }


        private async Task<List<KeqingNiuzaFileInfo>> TestUpdate()
        {
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate });
            client.DefaultRequestHeaders.Add("User-Agent", $"KeqingLauncher/{MetaData.FileVersion} UserId/{MetaData.UserId}");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            var jsonTask = client.GetStringAsync(versionUrl);
            var wallpaperExist = File.Exists(".\\UserData\\setting_wallpaper");
            Task<string> wallpaperTask = null;
            if (wallpaperExist)
            {
                wallpaperTask = client.GetStringAsync(wallpaperUrl);
            }
            var fs = await Task.Run(() => Directory.GetFiles(".\\", "*", SearchOption.AllDirectories));
            var files = await Task.Run(() => fs.Select(x => new KeqingNiuzaFileInfo(x)).ToList());
            var versionInfo = JsonConvert.DeserializeObject<VersionInfo>(await jsonTask);
            if (wallpaperExist)
            {
                var wallpapers = JsonConvert.DeserializeObject<List<KeqingNiuzaFileInfo>>(await wallpaperTask);
                versionInfo.KeqingNiuzaFiles.AddRange(wallpapers);
            }
            versionInfo.KeqingNiuzaFiles.ForEach(x => x.Path = Path.GetFullPath(x.Path));
            if (versionInfo.Version != VersionText.Text)
            {
                VersionText.Text += $" -> {versionInfo.Version}";
            }
            return versionInfo.KeqingNiuzaFiles.Except(files).ToList();
        }



        private async Task DownloadFiles(List<KeqingNiuzaFileInfo> list)
        {
            _progressLoading.SetAnimationState(AnimationState.IndicatorAppear);
            _progressLoading.SetAnimationState(AnimationState.ProgressExpand);
            _progressLoading.SetAnimationState(AnimationState.ProgressShow);
            await Task.Delay(800);
            var baseDir = Path.GetDirectoryName(AppContext.BaseDirectory);
            var rootFiles = list.Where(x => Path.GetDirectoryName(x.Path) == baseDir);
            if ((bool)rootFiles?.Any())
            {

                if (Directory.Exists(".\\deleting"))
                {
                    Directory.Delete(".\\deleting", true);
                }
                Directory.CreateDirectory(".\\deleting");
                foreach (var item in rootFiles)
                {
                    if (File.Exists(item.Path))
                    {
                        File.Move(item.Path, $".\\deleting\\{item.Name}");
                    }
                }
            }
            _downloader = new Downloader();
            _downloader.ProgressChanged += (s, e) =>
            {
                _progressLoading.ProgressValue = (float)e.DownloadedSize / e.TotalSize;
                InfoTest = "正在下载文件";
                ProgressTest = $"{(float)e.DownloadedSize / e.TotalSize:P2}";
                SpeedTest = $"{LengthToString(e.DownloadedSize, e.TotalSize)}   {e.Speed / 1024} KB/s";
            };
            _downloader.DownloadFinished += (s, e) => { InfoTest = "下载完成"; ProgressTest = ""; SpeedTest = ""; };
            await _downloader.DownloadAsync(list);
            await Task.Delay(300);
            _progressLoading.SetAnimationState(AnimationState.ProgressClose);
            _progressLoading.SetAnimationState(AnimationState.IndicatorDisappear);
            await Task.Delay(800);
        }


        private string LengthToString(long current, long total)
        {
            if (total <= 1 << 20)
            {
                return $"{(double)current / (1 << 10):F2}/{(double)total / (1 << 10):F2} KB";
            }
            else
            {
                return $"{(double)current / (1 << 20):F2}/{(double)total / (1 << 20):F2} MB";
            }
        }


    }
}
