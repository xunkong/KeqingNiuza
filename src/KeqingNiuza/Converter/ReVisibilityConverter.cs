using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    public class ReVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = (Visibility)value;
            if (visible == Visibility.Visible)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
