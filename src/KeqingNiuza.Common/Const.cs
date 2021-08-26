using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Common
{
    public static class Const
    {


        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };




        static Const()
        {

        }


    }
}
