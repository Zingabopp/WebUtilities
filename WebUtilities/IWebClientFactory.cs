namespace WebUtilities
{
    /// <summary>
    /// Abstraction for an object that provides <see cref="IWebClient"/>s
    /// </summary>
    public interface IWebClientFactory
    {
        /// <summary>
        /// Returns the default <see cref="IWebClient"/>.
        /// </summary>
        /// <returns></returns>
        IWebClient GetDefaultClient();

        /// <summary>
        /// Gets an <see cref="IWebClient"/> for the given host.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        IWebClient GetWebClient(string host);
    }
}
