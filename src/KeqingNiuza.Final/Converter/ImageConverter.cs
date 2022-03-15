using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace KeqingNiuza.Converter
{
    public class ImageConverter : IValueConverter
    {

        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            if (File.Exists(str))
            {
                using (var fs = File.OpenRead(str))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = fs;
                    bitmap.EndInit();
                    return bitmap;
                }
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
