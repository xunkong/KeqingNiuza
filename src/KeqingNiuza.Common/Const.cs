using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Common
{
    public static class Const
    {
        public static readonly Version Version = new Version(0, 2, 0, 21061012);


        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        


        static Const()
        {

        }


    }
}
