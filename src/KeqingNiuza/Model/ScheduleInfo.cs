using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
                OnPropertyChanged("RemainingTime");
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
                OnPropertyChanged("NextTriggerTime");
            }
        }

        public void Refresh()
        {
            OnPropertyChanged("RemainingTime");
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
