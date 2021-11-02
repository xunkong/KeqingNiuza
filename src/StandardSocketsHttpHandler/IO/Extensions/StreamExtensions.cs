using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    internal static class StreamExtensions
    {
        public static Task<int> ReadAsync(this Stream stream, Memory<byte> memory)
        {
            return stream.ReadAsync(memory, CancellationToken.None);
        }

        public static Task<int> ReadAsync(this Stream stream, Memory<byte> memory, CancellationToken cancellationToken)
        {
            if (!MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> buffer))
            {
                new NotSupportedException("This Memory does not support exposing the underlying array.");
            }
            return stream.ReadAsync(buffer.Array, buffer.Offset, buffer.Count);
        }

        public static Task WriteAsync(this Stream stream, ReadOnlyMemory<byte> memory)
        {
            return stream.WriteAsync(memory, CancellationToken.None);
        }

        public static Task WriteAsync(this Stream stream, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken)
        {
            if (!MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> buffer))
            {
                new NotSupportedException("This Memory does not support exposing the underlying array.");
            }
            return stream.WriteAsync(buffer.Array, buffer.Offset, buffer.Count, cancellationToken);
        }
    }
}
