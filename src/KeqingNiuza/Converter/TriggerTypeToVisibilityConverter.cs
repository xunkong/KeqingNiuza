using KeqingNiuza.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    internal class TriggerTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (ScheduleInfoTriggerType)value;
            if (parameter.ToString().Contains(type.ToString()))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse<ScheduleInfoTriggerType>(parameter.ToString(), out var type))
            {
                return type;
            }
            else
            {
                return ScheduleInfoTriggerType.None;
            }
        }
    }
}
