using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebUtilities
{
    /// <summary>
    /// Extension methods for IWebClient
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The provided Uri is null.</exception>
        /// <exception cref="WebClientException">Thrown when an error occurs in the web client or the request times out.</exception>
        public static Task<IWebResponseMessage> GetAsync(this IWebClient client, Uri uri)
            => client.GetAsync(uri, 0, CancellationToken.None);


        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation. If the provided CancellationToken is triggered, the operation is canceled.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException">The provided Uri is null.</exception>
        /// <exception cref="WebClientException">Thrown when an error occurs in the web client or the request times out.</exception>
        /// <returns></returns>
        public static Task<IWebResponseMessage> GetAsync(this IWebClient client, Uri uri, CancellationToken cancellationToken)
            => client.GetAsync(uri, 0, cancellationToken);


        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation. If the server doesn't respond inside the provided timeout (milliseconds), the operation is canceled.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <exception cref="ArgumentNullException">The provided Uri is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="timeout"/> is less than 0.</exception>
        /// <exception cref="WebClientException">Thrown when an error occurs in the web client or the request times out.</exception>
        /// <returns></returns>
        public static Task<IWebResponseMessage> GetAsync(this IWebClient client, Uri uri, int timeout)
            => client.GetAsync(uri, timeout, CancellationToken.None);


        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The provided Uri is null.</exception>
        /// <exception cref="UriFormatException">Thrown when a URI cannot be formed from the provided URL.</exception>
        /// <exception cref="WebClientException">Thrown when an error occurs in the web client or the request times out.</exception>
        public static Task<IWebResponseMessage> GetAsync(this IWebClient client, string url)
            => client.GetAsync(new Uri(url), 0, CancellationToken.None);


        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation. If the provided CancellationToken is triggered, the operation is canceled.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException">The provided Uri is null.</exception>
        /// <exception cref="UriFormatException">Thrown when a URI cannot be formed from the provided URL.</exception>
        /// <exception cref="WebClientException">Thrown when an error occurs in the web client or the request times out.</exception>
        /// <returns></returns>
        public static Task<IWebResponseMessage> GetAsync(this IWebClient client, string url, CancellationToken cancellationToken)
            => client.GetAsync(new Uri(url), 0, cancellationToken);


        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation. If the server doesn't respond inside the provided timeout (milliseconds), the operation is canceled.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <exception cref="ArgumentNullException">The provided Uri is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="timeout"/> is less than 0.</exception>
        /// <exception cref="UriFormatException">Thrown when a URI cannot be formed from the provided URL.</exception>
        /// <exception cref="WebClientException">Thrown when an error occurs in the web client or the request times out.</exception>
        /// <returns></returns>
        public static Task<IWebResponseMessage> GetAsync(this IWebClient client, string url, int timeout)
            => client.GetAsync(new Uri(url), timeout, CancellationToken.None);
    }
}
