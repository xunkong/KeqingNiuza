namespace DGP.Genshin.Common.Response
{
    public enum KnownReturnCode
    {
        InternalFailure = int.MinValue,
        AlreadySignedIn = -5003,
        AuthKeyTimeOut = -101,
        OK = 0,
        NotDefined = 7,
    }
}
