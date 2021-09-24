using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebUtilities
{
    /// <summary>
    /// An interface defining content returned with an <see cref="IWebResponse"/>.
    /// </summary>
    public interface IWebResponseContent : IDisposable
    {
        /// <summary>
        /// Returns the content of the response as a string asynchronously.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no content to read.</exception>
        Task<string> ReadAsStringAsync();
        /// <summary>
        /// Returns the content of the response as a stream asynchronously.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no content to read.</exception>
        Task<Stream> ReadAsStreamAsync();
        /// <summary>
        /// Returns the content of the response in a byte array asynchronously.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no content to read.</exception>
        Task<byte[]> ReadAsByteArrayAsync();

        /// <summary>
        /// Type of content reported by the response headers, if available.
        /// </summary>
        string? ContentType { get; }
        /// <summary>
        /// Length of the content in bytes reported by the response headers, if available.
        /// </summary>
        long? ContentLength { get; }
        /// <summary>
        /// The response content headers.
        /// </summary>
        ReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

    }
}
