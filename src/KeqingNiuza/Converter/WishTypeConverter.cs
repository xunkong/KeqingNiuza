using KeqingNiuza.Wish;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    class WishTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (WishType)value;
            string result;
            switch (type)
            {
                case WishType.Novice:
                    result = "新手祈愿";
                    break;
                case WishType.Permanent:
                    result = "常驻祈愿";
                    break;
                case WishType.CharacterEvent:
                    result = "角色活动祈愿";
                    break;
                case WishType.WeaponEvent:
                    result = "武器活动祈愿";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
