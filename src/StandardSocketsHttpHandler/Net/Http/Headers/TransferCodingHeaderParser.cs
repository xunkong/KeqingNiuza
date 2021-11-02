// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Net.Http.Headers
{
    internal class TransferCodingHeaderParser : BaseHeaderParser
    {
        private Func<string, TransferCodingHeaderValue> _transferCodingCreator;

        internal static readonly TransferCodingHeaderParser SingleValueParser =
            new TransferCodingHeaderParser(false, CreateTransferCoding);
        internal static readonly TransferCodingHeaderParser MultipleValueParser =
            new TransferCodingHeaderParser(true, CreateTransferCoding);
        internal static readonly TransferCodingHeaderParser SingleValueWithQualityParser =
            new TransferCodingHeaderParser(false, CreateTransferCodingWithQuality);
        internal static readonly TransferCodingHeaderParser MultipleValueWithQualityParser =
            new TransferCodingHeaderParser(true, CreateTransferCodingWithQuality);

        private TransferCodingHeaderParser(bool supportsMultipleValues,
            Func<string, TransferCodingHeaderValue> transferCodingCreator)
            : base(supportsMultipleValues)
        {
            Debug.Assert(transferCodingCreator != null);

            _transferCodingCreator = transferCodingCreator;
        }

        private static TransferCodingHeaderValue CreateTransferCoding(string value)
        {
            return new TransferCodingHeaderValue(value);
        }

        private static TransferCodingHeaderValue CreateTransferCodingWithQuality(string value)
        {
            return new TransferCodingWithQualityHeaderValue(value);
        }
    }
}
