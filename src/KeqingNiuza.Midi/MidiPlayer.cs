using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Midi;

namespace KeqingNiuza.Midi
{
    public class MidiPlayer
    {


        /// <summary>
        /// Midi文件路径
        /// </summary>
        public string FilePath { get; private set; }


        /// <summary>
        /// Midi文件名
        /// </summary>
        public string FileName { get; private set; }


        /// <summary>
        /// 是否可以演奏
        /// </summary>
        public bool CanPlay { get; private set; }


        /// <summary>
        /// 演奏状态
        /// </summary>
        public PlayState PlayState { get; private set; }

        /// <summary>
        /// Midi文件总时间，以毫秒表示
        /// </summary>
        public long TotalTimeInMilliseconds { get; private set; }

        /// <summary>
        /// Midi文件正在演奏的时间点，以毫秒表示
        /// </summary>
        public long NowTimeInMilloseconds { get; private set; }





        /// <summary>
        /// 后台演奏线程
        /// </summary>
        private static Thread _playingThread;


        private MidiFile _midiFile;

        /// <summary>
        /// midi文件中每tick代表的毫秒数
        /// </summary>
        private double _millisecondsPerTicks;

        /// <summary>
        /// 系统时钟中当前时刻的tick
        /// </summary>
        private long nowTimestamp => Stopwatch.GetTimestamp();

        /// <summary>
        /// 系统时钟的频率
        /// </summary>
        private long frequency => Stopwatch.Frequency;

        /// <summary>
        /// 此轮演奏开始时系统时钟的tick
        /// </summary>
        private long _startTimestamp;

        /// <summary>
        /// 此轮演奏从文件中的哪一个时间点开始，以毫秒表示
        /// </summary>
        private long _midiStartPointInMilliseconds;


        public MidiPlayer()
        {

        }


        public void Play() { }


        public void Play(uint startSeconds) { }

        public void Pause() { }

        public void Stop() { }


        public void ChangeFile()
        {
            if (_playingThread != null)
            {
                _playingThread.Abort();
            }

        }


        private void PlayBackground()
        {
            switch (PlayState)
            {
                case PlayState.Stop:
                    break;
                case PlayState.Playing:
                    {

                    }
                    break;
                case PlayState.Pause:
                    Thread.Sleep(Timeout.Infinite);
                    break;
                default:
                    break;
            }
        }

    }
}
