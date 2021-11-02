using System.Runtime.CompilerServices;

#if NETSTANDARD2_1
// If an assembly was compiled against the .NET Standard 2.0 version of this library DLL but then this library DLL was replaced by the .NET Standard 2.1 version
// the CLR will look for SslClientAuthenticationOptions in the .NET Standard 2.1 version of this library DLL, we need to forward it to the right assembly.
[assembly: TypeForwardedTo(typeof(System.Net.Security.SslClientAuthenticationOptions))]
#endif