using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebUtilities
{
    /// <summary>
    /// An interface that defines a web client.
    /// </summary>
    public interface IWebClient : IDisposable
    {
        /// <summary>
        /// Default timeout for requests in milliseconds.
        /// </summary>
        int Timeout { get; set; }
        /// <summary>
        /// The UserAgent string the client sends with request headers. Must start with "PRODUCT/VERSION" (No '\').
        /// </summary>
        string? UserAgent { get; }
        /// <summary>
        /// How the WebClient handles errors. TODO: May not be fully implemented.
        /// </summary>
        ErrorHandling ErrorHandling { get; set; }
        /// <summary>
        /// Sets the UserAgent the client sends in the request headers.
        /// Should be in the format: Product/Version.
        /// </summary>
        /// <param name="userAgent"></param>
        void SetUserAgent(string? userAgent);

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation. If the server doesn't respond inside the provided timeout (milliseconds) or the provided CancellationToken is triggered, the operation is canceled.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException">The provided Uri is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="timeout"/> is less than 0.</exception>
        /// <exception cref="WebClientException">Thrown when an error occurs in the web client or the request times out.</exception>
        /// <exception cref="OperationCanceledException">The provided cancellationToken was triggered.</exception>
        /// <returns></returns>
        Task<IWebResponseMessage> GetAsync(Uri uri, int timeout, CancellationToken cancellationToken);
    }

    /// <summary>
    /// The type of error handling the <see cref="IWebClient"/> uses.
    /// </summary>
    public enum ErrorHandling
    {
        /// <summary>
        /// Any thrown exceptions are passed through to the caller.
        /// </summary>
        ThrowOnException,
        /// <summary>
        /// Any web faults with throw a <see cref="WebClientException"/>.
        /// </summary>
        ThrowOnWebFault,
        /// <summary>
        /// Any <see cref="Exception"/>s thrown are stored and returned in a <see cref="IWebResponse"/> with empty content.
        /// </summary>
        ReturnEmptyContent
    }
}
