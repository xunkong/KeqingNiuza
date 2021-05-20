using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    public class AvatarConverter : IValueConverter
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
                if (File.Exists("resource\\avatar\\Paimon.png"))
                {
                    var file = new FileInfo("resource\\avatar\\Paimon.png");
                    return file.FullName;
                }
                else
                {
                    return null;
                }
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }



    }
}
