using HandyControl.Controls;
using KeqingNiuza.Midi;
using KeqingNiuza.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Interop;

namespace KeqingNiuza.ViewModel
{
    public class MidiViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private ObservableCollection<MidiFileInfo> _MidiFileInfoList;
        public ObservableCollection<MidiFileInfo> MidiFileInfoList
        {
            get { return _MidiFileInfoList; }
            set
            {
                _MidiFileInfoList = value;
                OnPropertyChanged();
            }
        }


        private MidiFileInfo _SelectedMidiFile;
        public MidiFileInfo SelectedMidiFile
        {
            get { return _SelectedMidiFile; }
            set
            {
                _SelectedMidiFile = value;
                MidiPlayer.ChangeFile(_SelectedMidiFile);
                OnPropertyChanged();
            }
        }


        private static MidiPlayer _MidiPlayer;
        public MidiPlayer MidiPlayer
        {
            get { return _MidiPlayer; }
            set
            {
                _MidiPlayer = value;
                OnPropertyChanged();
            }
        }


        private bool _IsAdmin;
        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set
            {
                _IsAdmin = value;
                OnPropertyChanged();
            }
        }


        private bool _CanPlay;
        public bool CanPlay
        {
            get { return _CanPlay; }
            set
            {
                _CanPlay = value;
                OnPropertyChanged();
            }
        }

        private string _StateText;
        public string StateText
        {
            get { return _StateText; }
            set
            {
                _StateText = value;
                OnPropertyChanged();
            }
        }



        private string _Button_Restart_Content;
        public string Button_Restart_Content
        {
            get { return _Button_Restart_Content; }
            set
            {
                _Button_Restart_Content = value;
                OnPropertyChanged();
            }
        }



        private string _TextBlock_Color;
        public string TextBlock_Color
        {
            get { return _TextBlock_Color; }
            set
            {
                _TextBlock_Color = value;
                OnPropertyChanged();
            }
        }


        private string _Tooltip_Content;
        public string Tooltip_Content
        {
            get { return _Tooltip_Content; }
            set
            {
                _Tooltip_Content = value;
                OnPropertyChanged();
            }
        }

        private bool hotkey;
        private bool disposedValue;
        private readonly IntPtr hWnd;
        private readonly HwndSource hwndSource;

        public MidiViewModel()
        {
            var files = Directory.GetFiles("resource\\midi").ToList();
            var infos = files.ConvertAll(x => new MidiFileInfo(x)).OrderBy(x => x.Name);
            MidiFileInfoList = new ObservableCollection<MidiFileInfo>(infos);
            MidiPlayer = new MidiPlayer("YuanShen");
            SelectedMidiFile = MidiFileInfoList.First();
            hWnd = Process.GetCurrentProcess().MainWindowHandle;
            hotkey = Util.RegisterHotKey(hWnd);
            hwndSource = HwndSource.FromHwnd(hWnd);
            hwndSource.AddHook(HwndHook);
            RefreshState();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _MidiPlayer?.Dispose();
                }
                Util.UnregisterHotKey(hWnd);
                hwndSource.RemoveHook(HwndHook);
                MidiFileInfoList = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~MidiViewModel()
        {
            Dispose(false);
        }


        public void ChangeFileAndPlay(MidiFileInfo info)
        {
            _MidiPlayer.ChangeFileAndPlay(info);
        }


        public void RefreshState()
        {
            IsAdmin = Util.IsAdmin();
            CanPlay = MidiPlayer.CanPlay;
            StateText = "正常";
            TextBlock_Color = "Black";
            Button_Restart_Content = null;
            Tooltip_Content = null;
            if (!hotkey)
            {
                StateText = "热键定义失败";
                TextBlock_Color = "Red";
                Button_Restart_Content = "重试";
                Tooltip_Content = null;
            }
            if (!CanPlay)
            {
                StateText = "没有找到原神的窗口";
                TextBlock_Color = "Red";
                Button_Restart_Content = "刷新";
                Tooltip_Content = "请打开游戏后点击刷新";
            }
            if (!IsAdmin)
            {
                StateText = "需要管理员权限";
                TextBlock_Color = "Red";
                Button_Restart_Content = "重启";
                Tooltip_Content = "软件会用管理员权限干什么？\n《原神》以管理员权限启动，软件需要管理员权限才能向游戏窗口发送键盘信息";
            }
        }


        public void RestartOrRefresh()
        {
            if (!IsAdmin)
            {
                try
                {
                    Util.RestartAsAdmin();
                }
                catch (Exception ex)
                {
                    Log.OutputLog(LogType.Error, "RestartAsAdmin", ex);
                    Growl.Error("无法重启，请手动以管理员权限启动");
                }
            }
            if (!CanPlay)
            {
                MidiPlayer?.Dispose();
                MidiPlayer = new MidiPlayer("YuanShen");
                SelectedMidiFile = MidiFileInfoList.First();
                RefreshState();
            }
            if (!hotkey)
            {
                _ = Util.UnregisterHotKey(hWnd);
                hotkey = Util.RegisterHotKey(hWnd);
                RefreshState();
            }
        }


        private IntPtr HwndHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_KOTKEY = 0x0312;
            if (msg == WM_KOTKEY)
            {
                switch (wParam.ToInt32())
                {
                    case 1000:
                        if (MidiPlayer.IsPlaying)
                        {
                            MidiPlayer.IsPlaying = false;
                        }
                        else
                        {
                            if (MidiPlayer.MidiFileInfo == null)
                            {
                                MidiPlayer.ChangeFileAndPlay(MidiFileInfoList.First());
                            }
                            else
                            {
                                MidiPlayer.IsPlaying = true;
                            }
                        }
                        handled = true;
                        break;
                    case 1001:
                        PlayLast();
                        handled = true;
                        break;
                    case 1002:
                        PlayNext();
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }


        public void PlayLast()
        {
            if (MidiPlayer.MidiFileInfo == null)
            {
                MidiPlayer.ChangeFileAndPlay(MidiFileInfoList.Last());
            }
            else
            {
                var index = MidiFileInfoList.IndexOf(MidiPlayer.MidiFileInfo);
                if (index == 0)
                {
                    MidiPlayer.ChangeFileAndPlay(MidiFileInfoList.Last());
                }
                else
                {
                    MidiPlayer.ChangeFileAndPlay(MidiFileInfoList[index - 1]);
                }
            }
        }


        public void PlayNext()
        {
            if (MidiPlayer.MidiFileInfo == null)
            {
                MidiPlayer.ChangeFileAndPlay(MidiFileInfoList.First());
            }
            else
            {
                var index = MidiFileInfoList.IndexOf(MidiPlayer.MidiFileInfo);
                if (index == MidiFileInfoList.Count - 1)
                {
                    MidiPlayer.ChangeFileAndPlay(MidiFileInfoList.First());
                }
                else
                {
                    MidiPlayer.ChangeFileAndPlay(MidiFileInfoList[index + 1]);
                }
            }
        }


        public void MoveToTime(int milliSeconds)
        {
            MidiPlayer?.MoveToTime(milliSeconds);
        }


    }
}
