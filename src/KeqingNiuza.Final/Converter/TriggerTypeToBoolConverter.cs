using KeqingNiuza.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    internal class TriggerTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (ScheduleInfoTriggerType)value;
            if (type.ToString() == parameter.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                if (Enum.TryParse<ScheduleInfoTriggerType>(parameter.ToString(), out var type))
                {
                    return type;
                }
                else
                {
                    return ScheduleInfoTriggerType.Countdown;
                }
            }
            else
            {
                return ScheduleInfoTriggerType.None;
            }

        }
    }
}
