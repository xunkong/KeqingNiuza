using System;

namespace System.Net.Http
{
    internal static class HttpResponseMessageExtensions
    {
        internal static void SetVersionWithoutValidation(this HttpResponseMessage message, Version value)
        {
            message.Version = value;
        }

        internal static void SetStatusCodeWithoutValidation(this HttpResponseMessage message, HttpStatusCode value)
        {
            message.StatusCode = value;
        }

        internal static void SetReasonPhraseWithoutValidation(this HttpResponseMessage message, string value)
        {
            message.ReasonPhrase = value;
        }
    }
}
