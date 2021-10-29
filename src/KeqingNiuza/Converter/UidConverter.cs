using System;
using System.Globalization;
using KeqingNiuza.Model;

namespace KeqingNiuza.Converter
{
    class UidConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "点此加载数据";
            }
            else
            {
                var userdata = value as UserData;
                return $"*****{userdata.Uid % 1000:D3}";
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
