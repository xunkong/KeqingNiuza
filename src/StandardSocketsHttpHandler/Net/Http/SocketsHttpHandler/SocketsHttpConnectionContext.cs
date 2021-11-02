// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Net.Http
{
    /// <summary>
    /// Represents the context passed to the ConnectCallback for a SocketsHttpHandler instance.
    /// </summary>
    public sealed class SocketsHttpConnectionContext
    {
        private readonly DnsEndPoint _dnsEndPoint;

        internal SocketsHttpConnectionContext(DnsEndPoint dnsEndPoint)
        {
            _dnsEndPoint = dnsEndPoint;
        }

        /// <summary>
        /// The DnsEndPoint to be used by the ConnectCallback to establish the connection.
        /// </summary>
        public DnsEndPoint DnsEndPoint => _dnsEndPoint;
    }
}
