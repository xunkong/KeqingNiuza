using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    public class ImageConverter : IValueConverter
    {

        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            if (File.Exists(str))
            {
                var file = new FileInfo(str);
                return file.FullName;
            }
            else
            {
                return null;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }



    }
}
