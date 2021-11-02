// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace System.Net.Http.Headers
{
    internal static class HeaderUtilities
    {
        private static readonly char[] s_hexUpperChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        internal static bool IsInputEncoded5987(string input, out string output)
        {
            // Encode a string using RFC 5987 encoding.
            // encoding'lang'PercentEncodedSpecials
            bool wasEncoded = false;
            StringBuilder builder = StringBuilderCache.Acquire();
            builder.Append("utf-8\'\'");
            foreach (char c in input)
            {
                // attr-char = ALPHA / DIGIT / "!" / "#" / "$" / "&" / "+" / "-" / "." / "^" / "_" / "`" / "|" / "~"
                //      ; token except ( "*" / "'" / "%" )
                if (c > 0x7F) // Encodes as multiple utf-8 bytes
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(c.ToString());
                    foreach (byte b in bytes)
                    {
                        AddHexEscaped((char)b, builder);
                        wasEncoded = true;
                    }
                }
                else if (!HttpRuleParser.IsTokenChar(c) || c == '*' || c == '\'' || c == '%')
                {
                    // ASCII - Only one encoded byte.
                    AddHexEscaped(c, builder);
                    wasEncoded = true;
                }
                else
                {
                    builder.Append(c);
                }

            }

            output = StringBuilderCache.GetStringAndRelease(builder);
            return wasEncoded;
        }

        /// <summary>Transforms an ASCII character into its hexadecimal representation, adding the characters to a StringBuilder.</summary>
        private static void AddHexEscaped(char c, StringBuilder destination)
        {
            Debug.Assert(destination != null);
            Debug.Assert(c <= 0xFF);

            destination.Append('%');
            destination.Append(s_hexUpperChars[(c & 0xf0) >> 4]);
            destination.Append(s_hexUpperChars[c & 0xf]);
        }
    }
}
