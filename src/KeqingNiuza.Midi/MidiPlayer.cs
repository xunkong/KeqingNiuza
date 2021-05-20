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
using System.Threading;
using System.Threading.Tasks;

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


        /// <summary>
        /// Midi文件名
        /// </summary>
        private string _Name;
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


        /// <summary>
        /// 演奏状态
        /// </summary>
        private bool _IsPlaying;
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
                    _timeWatcher.Stop();
                }
                _IsPlaying = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Midi文件总时间，以秒为单位
        /// </summary>
        private TimeSpan _TotalTime;
        public TimeSpan TotalTime
        {
            get { return _TotalTime; }
            set
            {
                _TotalTime = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Midi文件正在演奏的时间点，以秒单位
        /// </summary>
        private TimeSpan _CurrentTime;
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
        public bool AllowPlayBackground
        {
            get { return _AllowPlayBackground; }
            set
            {
                _AllowPlayBackground = value;
                OnPropertyChanged();
            }
        }


        private bool _AutoSwitchToGenshinWindow;
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
                    _timeWatcher.PollingInterval = new TimeSpan(0, 0, 0, 0, (int)(1000 / value));
                    _Speed = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _NoteLevel = 0;
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

        private Process _process;

        private IntPtr _hWnd;

        private PlaybackCurrentTimeWatcher _timeWatcher;


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
            _timeWatcher = PlaybackCurrentTimeWatcher.Instance;
            _timeWatcher.PollingInterval = new TimeSpan(0, 0, 1);
            _timeWatcher.CurrentTimeChanged += _watcher_CurrentTimeChanged;
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
                _timeWatcher?.Dispose();
            }
            disposed = true;
        }



        private void _watcher_CurrentTimeChanged(object sender, PlaybackCurrentTimeChangedEventArgs e)
        {
            CurrentTime = e.Times.First().Time as MetricTimeSpan;
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

        public void Play(int startSeconds)
        {
            var time = new MetricTimeSpan(0, 0, startSeconds);
            if (_playback.IsRunning)
            {
                _playback.MoveToTime(time);
            }
            else
            {
                _playback.PlaybackStart = time;

                _playback.Start();

            }

        }



        private void _playback_Stopped(object sender, EventArgs e)
        {
            IsPlaying = false;
            _timeWatcher.Stop();
        }

        private void _playback_Started(object sender, EventArgs e)
        {
            IsPlaying = true;
            _timeWatcher.Start();
        }

        public void ChangeFile(MidiFileInfo info)
        {
            MidiFileInfo = info;
            Name = info.Name;
            CurrentTime = new TimeSpan();
            _playback?.Dispose();
            TotalTime = info.MidiFile.GetDuration<MetricTimeSpan>();
            _playback = info.MidiFile.GetPlayback();
            _playback.Speed = Speed;
            _playback.InterruptNotesOnStop = true;
            _playback.EventPlayed += NoteEventPlayed;
            _playback.Started += _playback_Started;
            _playback.Stopped += _playback_Stopped;
            _playback.Finished += _playback_Finished;
            _timeWatcher.RemoveAllPlaybacks();
            _timeWatcher.AddPlayback(_playback, TimeSpanType.Metric);
        }

        public void ChangeFileAndPlay(MidiFileInfo info)
        {
            ChangeFile(info);
            IsPlaying = true;
        }

        private void _playback_Finished(object sender, EventArgs e)
        {
            IsPlaying = false;
            _timeWatcher.Stop();
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
