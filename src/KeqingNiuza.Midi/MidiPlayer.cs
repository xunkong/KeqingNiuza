using KeqingNiuza.Midi.Native;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Devices;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;

namespace KeqingNiuza.Midi
{
    public class MidiPlayer : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public MidiFileInfo MidiFileInfo { get; private set; }


        private string _Name;
        /// <summary>
        /// Midi文件名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// 是否可以演奏
        /// </summary>
        public bool CanPlay { get; private set; }


        private bool _IsPlaying;
        /// <summary>
        /// 演奏状态
        /// </summary>
        public bool IsPlaying
        {
            get { return _IsPlaying; }
            set
            {
                if (_playback == null)
                {
                    return;
                }
                if (_IsPlaying == value)
                {
                    return;
                }
                if (value)
                {
                    if (AutoSwitchToGenshinWindow)
                    {
                        User32.SwitchToThisWindow(_hWnd, true);
                        Thread.Sleep(100);
                    }
                    _playback.Start();
                }
                else
                {
                    _playback.Stop();
                }
                _IsPlaying = value;
                OnPropertyChanged();
            }
        }


        private TimeSpan _TotalTime;
        /// <summary>
        /// Midi文件总时间，以秒为单位
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return _TotalTime; }
            set
            {
                _TotalTime = value;
                OnPropertyChanged();
            }
        }


        private TimeSpan _CurrentTime;
        /// <summary>
        /// Midi文件正在演奏的时间点，以秒单位
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return _CurrentTime; }
            set
            {
                _CurrentTime = value;
                _playback?.MoveToTime(new MetricTimeSpan(value));
                OnPropertyChanged();
            }
        }


        private bool _AllowPlayBackground;
        /// <summary>
        /// 后台播放
        /// </summary>
        public bool AllowPlayBackground
        {
            get { return _AllowPlayBackground; }
            set
            {
                _AllowPlayBackground = value;
                OnPropertyChanged();
            }
        }


        private bool _AutoSwitchToGenshinWindow = true;
        /// <summary>
        /// 自动跳转到游戏窗口
        /// </summary>
        public bool AutoSwitchToGenshinWindow
        {
            get { return _AutoSwitchToGenshinWindow; }
            set
            {
                _AutoSwitchToGenshinWindow = value;
                OnPropertyChanged();
            }
        }





        private double _Speed = 1;
        /// <summary>
        /// 速度
        /// </summary>
        public double Speed
        {
            get { return _Speed; }
            set
            {
                if (_playback == null || IsPlaying)
                {
                    return;
                }
                if (value > 0)
                {
                    _playback.Speed = value;
                    _timer.Interval = 1000 / value;
                    _Speed = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _NoteLevel = 0;
        /// <summary>
        /// 升降调
        /// </summary>
        public int NoteLevel
        {
            get { return _NoteLevel; }
            set
            {
                _NoteLevel = value;
                OnPropertyChanged();
            }
        }


        private bool disposed = false;

        private Playback _playback;

        private readonly Process _process;

        private readonly IntPtr _hWnd;

        private readonly System.Timers.Timer _timer;

        public MidiPlayer(string processName)
        {
            var pros = Process.GetProcessesByName(processName);
            if (pros.Any())
            {
                _process = pros[0];
                CanPlay = true;
                _process.Exited += GenshinExited;
                _hWnd = _process.MainWindowHandle;
            }
            _timer = new System.Timers.Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
        }


        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CurrentTime = _playback.GetCurrentTime<MetricTimeSpan>();
        }

        ~MidiPlayer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                _playback?.Dispose();
                _timer?.Dispose();
            }
            disposed = true;
        }




        private void GenshinExited(object sender, EventArgs e)
        {
            CanPlay = false;
            _playback?.Dispose();
        }

        public void Play()
        {
            _playback?.Start();
        }

        public void MoveToTime(int milliSeconds)
        {
            var time = new MetricTimeSpan(0, 0, 0, milliSeconds);
            _playback?.MoveToTime(time);
        }



        private void _playback_Stopped(object sender, EventArgs e)
        {
            IsPlaying = false;
            _timer.Stop();
        }

        private void _playback_Started(object sender, EventArgs e)
        {
            IsPlaying = true;
            _timer.Start();
        }

        public void ChangeFile(MidiFileInfo info)
        {
            MidiFileInfo = info;
            Name = info.Name;
            CurrentTime = new TimeSpan();
            _playback?.Dispose();
            _timer.Stop();
            TotalTime = info.MidiFile.GetDuration<MetricTimeSpan>();
            _playback = info.MidiFile.GetPlayback();
            _playback.Speed = Speed;
            _playback.InterruptNotesOnStop = true;
            _playback.EventPlayed += NoteEventPlayed;
            _playback.Started += _playback_Started;
            _playback.Stopped += _playback_Stopped;
            _playback.Finished += _playback_Finished;
        }

        public void ChangeFileAndPlay(MidiFileInfo info)
        {
            ChangeFile(info);
            IsPlaying = true;
        }

        private void _playback_Finished(object sender, EventArgs e)
        {
            IsPlaying = false;
            _timer.Stop();
            CurrentTime = new TimeSpan();
        }

        private void NoteEventPlayed(object sender, MidiEventPlayedEventArgs e)
        {
            if (e.Event.EventType == MidiEventType.NoteOn)
            {
                var note = e.Event as NoteOnEvent;
                var num = (int)note.NoteNumber + NoteLevel;
                while (true)
                {
                    if (num < 48)
                    {
                        num += 12;
                    }
                    if (num > 83)
                    {
                        num -= 12;
                    }
                    if (num >= 48 || num <= 83)
                    {
                        break;
                    }
                }
                if (Const.NoteToVisualKeyDictionary.ContainsKey(num))
                {
                    Util.Postkey(_hWnd, num, AllowPlayBackground);
                }
            }
        }





    }
}
