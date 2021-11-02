namespace DGP.Genshin.Common.Request.QueryString
{
    /// <summary>
    /// Specifies the separator to be used between query string parameters.
    /// </summary>
    public enum QueryStringSeparator
    {
        /// <summary>
        /// The default separator for query string parameters. Generated query string is like "a=1&b=5".
        /// </summary>
        Ampersand,

        /// <summary>
        /// An alternative separator for query string parameters. Generated query string is like "a=1;b=5".
        /// </summary>
        Semicolon
    }
}
