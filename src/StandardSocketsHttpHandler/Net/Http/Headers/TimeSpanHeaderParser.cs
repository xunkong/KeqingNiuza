// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Globalization;

namespace System.Net.Http.Headers
{
    internal class TimeSpanHeaderParser : BaseHeaderParser
    {
        internal static readonly TimeSpanHeaderParser Parser = new TimeSpanHeaderParser();

        private TimeSpanHeaderParser()
            : base(false)
        {
        }

        public override string ToString(object value)
        {
            Debug.Assert(value is TimeSpan);

            return ((int)((TimeSpan)value).TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}
