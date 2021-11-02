// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal abstract class HttpContentDuplexStream : HttpContentStream
    {
        public HttpContentDuplexStream(HttpConnection connection) : base(connection)
        {
            if (NetEventSource.IsEnabled) NetEventSource.Info(this);
        }

        public sealed override bool CanRead => true;
        public sealed override bool CanWrite => true;

        public sealed override void Flush() => FlushAsync().GetAwaiter().GetResult();

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            ValidateBufferArgs(buffer, offset, count);
            return ReadAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ValidateBufferArgs(buffer, offset, count);
            return base.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            ValidateBufferArgs(buffer, offset, count);
            WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ValidateBufferArgs(buffer, offset, count);
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }
    }
}
