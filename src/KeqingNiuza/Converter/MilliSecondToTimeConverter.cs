using System;
using System.Globalization;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    public class MilliSecondToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {

            var ms = (double)value;
            var m = (int)ms / 60000;
            var s = (int)ms / 1000 % 60;
            return $"{m}:{s:00}";

            //var time = (TimeSpan)value;
            //var ms = time.Seconds;
            //var m = (int)ms / 60;
            //var s = (int)ms % 60;
            //return $"{m}:{s:00}";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
