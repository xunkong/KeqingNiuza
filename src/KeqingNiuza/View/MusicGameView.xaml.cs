using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static KeqingNiuza.Core.MusicGame.MusicGameUtil;
using System.Windows.Shapes;
using KeqingNiuza.Core.MusicGame;
using System.Windows.Interop;
using KeqingNiuza.Core.Midi;
using System.Threading;
using System.IO;
using System.Text.Json;

namespace KeqingNiuza.View
{
    /// <summary>
    /// MusicGameView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicGameView : UserControl, IDisposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private IntPtr _hwnd_keqing;
        private IntPtr _hwnd_genshin;
        private HwndSource _hwndSource;


        // 时间相对偏移量(ms)，正数为快进，负数为快退
        private long _releativeTime;

        private List<NoteInfo> _noteList;

        private Queue<NoteInfo> _noteQueue;


        private bool _IsFindGenshinWindow;
        public bool IsFindGenshinWindow
        {
            get { return _IsFindGenshinWindow; }
            set
            {
                _IsFindGenshinWindow = value;
                OnPropertyChanged();
            }
        }

        private bool _IsRegisterHotkey;
        public bool IsRegisterHotkey
        {
            get { return _IsRegisterHotkey; }
            set
            {
                _IsRegisterHotkey = value;
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


        private bool _IsPlaying;
        private bool disposedValue;

        public bool IsPlaying
        {
            get { return _IsPlaying; }
            set
            {
                _IsPlaying = value;
                OnPropertyChanged();
            }
        }



        public MusicGameView()
        {
            InitializeComponent();
            _hwnd_keqing = Process.GetCurrentProcess().MainWindowHandle;
            var ps = Process.GetProcessesByName("YuanShen").ToList();
            ps.AddRange(Process.GetProcessesByName("GenshinImpact"));
            if (ps.Any())
            {
                _hwnd_genshin = ps[0].MainWindowHandle;
                IsFindGenshinWindow = true;
            }
            if (MusicGameUtil.IsAdmin())
            {
                IsAdmin = true;
            }
            if (RegisterHotKey(_hwnd_keqing))
            {
                IsRegisterHotkey = true;
            }
            _hwndSource = HwndSource.FromHwnd(_hwnd_keqing);
            _hwndSource.AddHook(MusicGameHwndHook);
            var path = @"E:\SnowMountain_hard.json";
            var json = File.ReadAllText(path);
            _noteList = JsonSerializer.Deserialize<List<NoteInfo>>(json);
        }


        private IntPtr MusicGameHwndHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_KOTKEY = 0x0312;
            if (msg == WM_KOTKEY)
            {
                switch (wParam.ToInt32())
                {
                    // 播放/停止
                    case 1003:
                        // todo
                        if (IsPlaying)
                        {
                            IsPlaying = false;
                        }
                        else
                        {
                            IsPlaying = true;
                            // todo
                            var t = new Thread(new ThreadStart(Play));
                            t.IsBackground = true;
                            t.Start();
                        }
                        handled = true;
                        break;
                    // 快退100ms
                    case 1004:
                        _releativeTime -= 100;
                        handled = true;
                        break;
                    // 快进100ms
                    case 1005:
                        _releativeTime += 100;
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }


        private async void Play()
        {
            Console.WriteLine($"{DateTime.Now}  start");
            _releativeTime = 0;
            var sw = Stopwatch.StartNew();
            _noteQueue = new Queue<NoteInfo>(_noteList);
            var notes = new List<NoteInfo>(8);
            Core.Native.User32.timeBeginPeriod(1);
            for (; ; )
            {
                if (!IsPlaying)
                {
                    break;
                }
                var time = _noteQueue.Peek().Time;
                while (_noteQueue.Any() && _noteQueue.FirstOrDefault()?.Time == time)
                {
                    notes.Add(_noteQueue.Dequeue());
                };
                var delay = (int)(time + _releativeTime - sw.ElapsedMilliseconds);
                await Task.Delay(delay > 0 ? delay : 0);
                notes.ForEach(x => PostKey(_hwnd_genshin, x.ButtonType, OperationType.KeyDownUp, false));
                Console.WriteLine(sw.ElapsedMilliseconds);
                foreach (var item in notes)
                {
                    Console.Write(item.ButtonType + "  ");
                }
                Console.WriteLine();
                notes.Clear();
                if (!_noteQueue.Any())
                {
                    break;
                }
            }
            IsPlaying = false;
            Core.Native.User32.timeEndPeriod(1);
            Console.WriteLine($"{DateTime.Now}  end");
        }


        #region IDispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    _hwndSource.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                UnregisterHotKey(_hwnd_keqing);
                disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~MusicGameView()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion


    }
}
