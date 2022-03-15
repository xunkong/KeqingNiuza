using System;
using System.Globalization;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    class TimeSpanToSliderValueConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {

            var time = (TimeSpan)value;
            return time.TotalMilliseconds;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ms = (double)value;
            var time = new TimeSpan(0, 0, 0, 0, (int)ms);
            return time;
        }
    }
}
