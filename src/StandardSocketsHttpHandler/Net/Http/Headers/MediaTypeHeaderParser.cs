// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Net.Http.Headers
{
    internal class MediaTypeHeaderParser : BaseHeaderParser
    {
        private bool _supportsMultipleValues;
        private Func<string, MediaTypeHeaderValue> _mediaTypeCreator;

        internal static readonly MediaTypeHeaderParser SingleValueParser = new MediaTypeHeaderParser(false,
            CreateMediaType);
        internal static readonly MediaTypeHeaderParser SingleValueWithQualityParser = new MediaTypeHeaderParser(false,
            CreateMediaTypeWithQuality);
        internal static readonly MediaTypeHeaderParser MultipleValuesParser = new MediaTypeHeaderParser(true,
            CreateMediaTypeWithQuality);

        private MediaTypeHeaderParser(bool supportsMultipleValues, Func<string, MediaTypeHeaderValue> mediaTypeCreator)
            : base(supportsMultipleValues)
        {
            Debug.Assert(mediaTypeCreator != null);

            _supportsMultipleValues = supportsMultipleValues;
            _mediaTypeCreator = mediaTypeCreator;
        }

        private static MediaTypeHeaderValue CreateMediaType(string mediaType)
        {
            return new MediaTypeHeaderValue(mediaType);
        }

        private static MediaTypeHeaderValue CreateMediaTypeWithQuality(string mediaType)
        {
            return new MediaTypeWithQualityHeaderValue(mediaType);
        }
    }
}
