using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using KeqingNiuza.Core.Native;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Devices;
using Melanchall.DryWetMidi.Interaction;

namespace KeqingNiuza.Core.Midi
{
    public class MidiPlayer
    {



        public MidiFileInfo MidiFileInfo { get; private set; }


        /// <summary>
        /// Midi文件名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 是否可以演奏
        /// </summary>
        public bool CanPlay { get; private set; }


        /// <summary>
        /// 演奏状态
        /// </summary>
        public bool IsPlaying
        {
            get { return _playback.IsRunning; }
            set
            {
                if (_playback == null)
                {
                    return;
                }
                if (IsPlaying == value)
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
            }
        }


        /// <summary>
        /// Midi文件总时间
        /// </summary>
        public TimeSpan TotalTime { get; private set; }


        /// <summary>
        /// Midi文件正在演奏的时间点
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return _playback.GetCurrentTime<MetricTimeSpan>(); }
            set { _playback.MoveToTime(new MetricTimeSpan(value)); }
        }


        /// <summary>
        /// 后台播放
        /// </summary>
        public bool PlayBackground { get; set; }


        /// <summary>
        /// 自动跳转到游戏窗口
        /// </summary>
        public bool AutoSwitchToGenshinWindow { get; set; } = true;




        /// <summary>
        /// 速度
        /// </summary>
        public double Speed
        {
            get { return _playback.Speed; }
            set
            {
                if (_playback == null || IsPlaying)
                {
                    return;
                }
                if (value > 0)
                {
                    _playback.Speed = value;
                }
            }
        }

        /// <summary>
        /// 升降调
        /// </summary>
        public int NoteLevel { get; set; }



        public event EventHandler Started;

        public event EventHandler Stopped;

        public event EventHandler Finished;

        private void _playback_Started(object sender, EventArgs e)
        {
            Started?.Invoke(this, e);
        }

        private void _playback_Stopped(object sender, EventArgs e)
        {
            Stopped?.Invoke(this, e);
        }

        private void _playback_Finished(object sender, EventArgs e)
        {
            Finished?.Invoke(this, e);
        }

        private Playback _playback;

        private readonly IntPtr _hWnd;


        public MidiPlayer(string processName)
        {
            var pros = Process.GetProcessesByName(processName);
            if (pros.Any())
            {
                _hWnd = pros[0].MainWindowHandle;
                CanPlay = true;
            }
        }

        public MidiPlayer()
        {
            var pros = Process.GetProcessesByName("YuanShen").ToList();
            pros.AddRange(Process.GetProcessesByName("GenshinImpact"));
            if (pros.Any())
            {
                _hWnd = pros[0].MainWindowHandle;
                CanPlay = true;
            }
        }

        ~MidiPlayer()
        {
            _playback?.Dispose();
        }



        public void ChangeFileInfo(MidiFileInfo info, bool autoPlay = true)
        {
            Name = info.Name;
            MidiFileInfo = info;
            ChangeFileInfo();
            if (autoPlay)
            {
                IsPlaying = true;
            }
        }

        public void ChangeFileInfo(bool autoPlay = true)
        {
            var time = CurrentTime;
            ChangeFileInfo();
            if (autoPlay)
            {
                IsPlaying = true;
            }
            CurrentTime = time;
        }

        private void ChangeFileInfo()
        {
            var speed = _playback?.Speed;
            _playback?.Dispose();
            MidiFileInfo.MidiFile.Chunks.Clear();
            MidiFileInfo.MidiFile.Chunks.AddRange(MidiFileInfo.MidiTracks.Where(x => x.IsCheck).Select(x => x.Track));
            _playback = MidiFileInfo.MidiFile.GetPlayback();
            TotalTime = MidiFileInfo.MidiFile.GetDuration<MetricTimeSpan>();
            _playback.Speed = speed ?? 1;
            _playback.InterruptNotesOnStop = true;
            _playback.EventPlayed += NoteEventPlayed;
            _playback.Started += _playback_Started;
            _playback.Stopped += _playback_Stopped;
            _playback.Finished += _playback_Finished;
        }

        private void NoteEventPlayed(object sender, MidiEventPlayedEventArgs e)
        {
            if (e.Event.EventType == MidiEventType.NoteOn)
            {
                var note = e.Event as NoteOnEvent;
                var num = note.NoteNumber + NoteLevel;
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
                    Util.Postkey(_hWnd, num, PlayBackground);
                }
            }
        }





    }
}
