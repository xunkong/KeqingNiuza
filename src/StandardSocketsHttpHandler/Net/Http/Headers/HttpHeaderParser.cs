// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace System.Net.Http.Headers
{
    internal abstract class HttpHeaderParser
    {
        internal const string DefaultSeparator = ", ";

        private bool _supportsMultipleValues;
        private string _separator;

        public bool SupportsMultipleValues
        {
            get { return _supportsMultipleValues; }
        }

        public string Separator
        {
            get
            {
                Debug.Assert(_supportsMultipleValues);
                return _separator;
            }
        }

        // If ValueType implements Equals() as required, there is no need to provide a comparer. A comparer is needed
        // e.g. if we want to compare strings using case-insensitive comparison.
        public virtual IEqualityComparer Comparer
        {
            get { return null; }
        }

        protected HttpHeaderParser(bool supportsMultipleValues)
        {
            _supportsMultipleValues = supportsMultipleValues;

            if (supportsMultipleValues)
            {
                _separator = DefaultSeparator;
            }
        }

        protected HttpHeaderParser(bool supportsMultipleValues, string separator)
        {
            Debug.Assert(!string.IsNullOrEmpty(separator));

            _supportsMultipleValues = supportsMultipleValues;
            _separator = separator;
        }

        // If ValueType is a custom header value type (e.g. NameValueHeaderValue) it already implements ToString() correctly.
        // However for existing types like int, byte[], DateTimeOffset we can't override ToString(). Therefore the 
        // parser provides a ToString() virtual method that can be overridden by derived types to correctly serialize 
        // values (e.g. byte[] to Base64 encoded string).
        public virtual string ToString(object value)
        {
            Debug.Assert(value != null);

            return value.ToString();
        }
    }
}
