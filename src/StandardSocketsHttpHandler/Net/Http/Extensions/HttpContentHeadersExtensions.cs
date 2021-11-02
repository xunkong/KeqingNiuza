using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace System.Net.Http
{
    internal static class HttpContentHeadersExtensions
    {
        public static void AddHeaders(this HttpContentHeaders headers, HttpContentHeaders sourceHeaders)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in sourceHeaders)
            {
                headers.Add(header.Key, header.Value);
            }
        }
    }
}
