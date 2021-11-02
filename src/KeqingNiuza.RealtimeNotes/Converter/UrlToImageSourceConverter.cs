using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace KeqingNiuza.RealtimeNotes.Converter
{
    internal class UrlToImageSourceConverter : IValueConverter
    {

        static HttpClient httpClient;

        static UrlToImageSourceConverter()
        {
            httpClient = new HttpClient(new StandardSocketsHttpHandler());
            httpClient.DefaultRequestHeaders.Host = "upload-bbs.mihoyo.com";
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36 Edg/95.0.1020.40");
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rs = httpClient.GetStreamAsync(value.ToString()).Result;
            var ms = new MemoryStream();
            rs.CopyTo(ms);
            ms.Position = 0;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
