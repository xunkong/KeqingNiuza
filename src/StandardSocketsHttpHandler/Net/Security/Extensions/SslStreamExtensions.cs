using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Security
{
    internal static class SslStreamExtensions
    {
        public static Task AuthenticateAsClientAsync(this SslStream sslStream, SslClientAuthenticationOptions sslClientAuthenticationOptions, CancellationToken cancellationToken)
        {
            return Task.Factory.FromAsync(
                BeginAuthenticateAsClient,
                iar => ((SslStream)iar.AsyncState).EndAuthenticateAsClient(iar),
                sslClientAuthenticationOptions, cancellationToken,
                sslStream);
        }

        public static IAsyncResult BeginAuthenticateAsClient(SslClientAuthenticationOptions sslClientAuthenticationOptions, CancellationToken cancellationToken, AsyncCallback asyncCallback, object asyncState)
        {
            // .NET Standard 2.0 and below
            bool checkCertificateRevocation = (sslClientAuthenticationOptions.CertificateRevocationCheckMode != X509RevocationMode.NoCheck);
            SslProtocols sslProtocols = sslClientAuthenticationOptions.EnabledSslProtocols;
            try
            {
                return ((SslStream)asyncState).BeginAuthenticateAsClient(sslClientAuthenticationOptions.TargetHost, sslClientAuthenticationOptions.ClientCertificates, sslProtocols, checkCertificateRevocation, asyncCallback, asyncState);
            }
            catch (ArgumentException e) when (e.ParamName == "sslProtocolType")
            {
                // .NET Framework prior to 4.7 will throw an exception when SslProtocols.None is provided to BeginAuthenticateAsClient.
                sslProtocols = SecurityProtocol.DefaultSecurityProtocols;
                return ((SslStream)asyncState).BeginAuthenticateAsClient(sslClientAuthenticationOptions.TargetHost, sslClientAuthenticationOptions.ClientCertificates, sslProtocols, checkCertificateRevocation, asyncCallback, asyncState);
            }
        }
    }
}
