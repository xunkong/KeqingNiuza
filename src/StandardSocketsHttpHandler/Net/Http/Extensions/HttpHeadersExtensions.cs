using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Net.Http.Headers
{
    internal static class HttpHeadersExtensions
    {
        internal static IEnumerable<KeyValuePair<HeaderDescriptor, string[]>> GetHeaderDescriptorsAndValues(this HttpHeaders headers)
        {
            List<KeyValuePair<HeaderDescriptor, string[]>> result = new List<KeyValuePair<HeaderDescriptor, string[]>>();
            foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
            {
                KnownHeader knownHeader = KnownHeaders.TryGetKnownHeader(header.Key);
                if (knownHeader == null)
                {
                    knownHeader = new KnownHeader(header.Key);
                }
                HeaderDescriptor descriptor = new HeaderDescriptor(knownHeader);
                result.Add(new KeyValuePair<HeaderDescriptor, string[]>(descriptor, header.Value.ToArray()));
            }

            return result;
        }
    }
}
