// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Net.Http.Headers
{
    // Don't derive from BaseHeaderParser since parsing the Base64 string is delegated to Convert.FromBase64String() 
    // which will remove leading, trailing, and whitespace in the middle of the string.
    internal class ByteArrayHeaderParser : HttpHeaderParser
    {
        internal static readonly ByteArrayHeaderParser Parser = new ByteArrayHeaderParser();

        private ByteArrayHeaderParser()
            : base(false)
        {
        }

        public override string ToString(object value)
        {
            Debug.Assert(value is byte[]);

            return Convert.ToBase64String((byte[])value);
        }
    }
}
