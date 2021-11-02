// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Diagnostics;

namespace System.Net.Http.Headers
{
    internal sealed class GenericHeaderParser : BaseHeaderParser
    {
        #region Parser instances

        internal static readonly GenericHeaderParser HostParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser TokenListParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser SingleValueNameValueWithParametersParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueNameValueWithParametersParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser SingleValueNameValueParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueNameValueParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser MailAddressParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser SingleValueProductParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueProductParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser RangeConditionParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser SingleValueAuthenticationParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueAuthenticationParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser RangeParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser RetryConditionParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser ContentRangeParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser ContentDispositionParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser SingleValueStringWithQualityParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueStringWithQualityParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser SingleValueEntityTagParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueEntityTagParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser SingleValueViaParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueViaParser = new GenericHeaderParser(true);
        internal static readonly GenericHeaderParser SingleValueWarningParser = new GenericHeaderParser(false);
        internal static readonly GenericHeaderParser MultipleValueWarningParser = new GenericHeaderParser(true);

        #endregion

        private GenericHeaderParser(bool supportsMultipleValues) : base(supportsMultipleValues)
        {
        }
    }
}
