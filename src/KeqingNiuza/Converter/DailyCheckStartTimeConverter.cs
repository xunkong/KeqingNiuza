using System;
using System.Globalization;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    class DailyCheckStartTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (DateTime)value;
            return time.ToString("HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var time = DateTime.Parse(value as string);
                return time;
            }
            catch { }
            return null;
        }
    }
}
