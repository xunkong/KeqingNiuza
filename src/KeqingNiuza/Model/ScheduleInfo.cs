using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Model
{
    public class ScheduleInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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


        private bool _IsEnable = true;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                _IsEnable = value;
                OnPropertyChanged();
            }
        }



        private ScheduleInfoTriggerType _TriggerType;
        public ScheduleInfoTriggerType TriggerType
        {
            get { return _TriggerType; }
            set
            {
                if (value == ScheduleInfoTriggerType.None)
                {
                    return;
                }
                _TriggerType = value;
                OnPropertyChanged();
            }
        }



        private DateTime _LastTriggerTime;
        public DateTime LastTriggerTime
        {
            get { return _LastTriggerTime; }
            set
            {
                _LastTriggerTime = value;
                OnPropertyChanged();
            }
        }



        private TimeSpan _Interval;
        [JsonConverter(typeof(IntervalJsonConverter))]
        public TimeSpan Interval
        {
            get { return _Interval; }
            set
            {
                _Interval = value;
                OnPropertyChanged();
            }
        }


        [JsonIgnore]
        public DateTime NextTriggerTime
        {
            get { return LastTriggerTime + Interval; }
            set
            {
                LastTriggerTime = value - Interval;
                OnPropertyChanged();
            }
        }


        [JsonIgnore]
        public TimeSpan RemainingTime
        {
            get
            {
                TimeSpan span = NextTriggerTime - DateTime.Now;
                if (span < new TimeSpan())
                {
                    return new TimeSpan();
                }
                else
                {
                    return span;
                }
            }
            set
            {
                LastTriggerTime = DateTime.Now + value - Interval;
                OnPropertyChanged();
            }
        }


        private int _MaxValue;
        public int MaxValue
        {
            get { return _MaxValue; }
            set
            {
                _MaxValue = value;
                OnPropertyChanged();
            }
        }


        private TimeSpan _TimePerValue;
        [JsonConverter(typeof(IntervalJsonConverter))]
        public TimeSpan TimePerValue
        {
            get { return _TimePerValue; }
            set
            {
                _TimePerValue = value;
                OnPropertyChanged();
            }
        }


        private DateTime _LastZeroValueTime;
        public DateTime LastZeroValueTime
        {
            get { return _LastZeroValueTime; }
            set
            {
                _LastZeroValueTime = value;
                OnPropertyChanged();
            }
        }


        [JsonIgnore]
        public int CurrentValue
        {
            get
            {
                int current = (int)((DateTime.Now - LastZeroValueTime).TotalSeconds / TimePerValue.TotalSeconds);
                return current > MaxValue ? MaxValue : current < 0 ? 0 : current;
            }
            set
            {
                // 值限制在最大最小范围内
                int current = value > MaxValue ? MaxValue : value < 0 ? 0 : value;
                LastZeroValueTime = DateTime.Now - new TimeSpan(TimePerValue.Ticks * current);
                OnPropertyChanged();
            }
        }


        [JsonIgnore]
        public DateTime NextMaxValueTime
        {
            get { return LastZeroValueTime + new TimeSpan(TimePerValue.Ticks * MaxValue); }
            set
            {
                LastZeroValueTime = value - new TimeSpan(TimePerValue.Ticks * MaxValue);
                OnPropertyChanged();
            }
        }


        private int _Custom_DelayDay;
        /// <summary>
        /// 下一次提醒时间在几天后
        /// </summary>
        public int Custom_DelayDay
        {
            get { return _Custom_DelayDay; }
            set
            {
                _Custom_DelayDay = value;
                OnPropertyChanged();
            }
        }


        private DateTime _Custom_TriggerTime;
        /// <summary>
        /// 提醒时在当天的几点
        /// </summary>
        public DateTime Custom_TriggerTime
        {
            get { return _Custom_TriggerTime; }
            set
            {
                _Custom_TriggerTime = value;
                OnPropertyChanged();
            }
        }




        public void Refresh()
        {
            OnPropertyChanged("RemainingTime");
            OnPropertyChanged("CurrentValue");
        }

    }

    class IntervalJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var ticks = reader.GetInt64();
            return new TimeSpan(ticks);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Ticks);
        }
    }
}
