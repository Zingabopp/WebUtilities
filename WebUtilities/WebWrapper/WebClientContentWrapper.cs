using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebUtilities.WebWrapper
{
    /// <summary>
    /// Wrapper for the content of a <see cref="WebClientResponseWrapper"/>.
    /// </summary>
    public class WebClientContent : IWebResponseContent
    {
        private HttpWebResponse? _response;
        /// <summary>
        /// Creates a new <see cref="WebClientContent"/> from the <paramref name="response"/>.
        /// </summary>
        /// <param name="response"></param>
        public WebClientContent(HttpWebResponse? response)
        {
            _response = response;
            ContentLength = _response?.ContentLength ?? 0;
            if (ContentLength < 0)
                ContentLength = null;
            _headers = new Dictionary<string, IEnumerable<string>>();
            if (_response?.Headers != null)
            {
                foreach (string? headerKey in _response.Headers.AllKeys)
                {
                    _headers.Add(headerKey, new string[] { _response.Headers[headerKey] });
                }
            }
        }

        /// <summary>
        /// Response headers.
        /// </summary>
        protected Dictionary<string, IEnumerable<string>> _headers;

        /// <summary>
        /// A ReadOnlyDictionary of the response headers.
        /// </summary>
        public ReadOnlyDictionary<string, IEnumerable<string>> Headers
        {
            get { return new ReadOnlyDictionary<string, IEnumerable<string>>(_headers); }
        }
        /// <inheritdoc/>
        public string ContentType
        {
            get
            {
                if (_response == null)
                    return string.Empty;
                string? cType = _response.ContentType ?? string.Empty;
                if (cType.Contains(";")) // ex: "text/html; charset=UTF-8"
                    cType = cType.Substring(0, cType.IndexOf(";"));
                return cType;
            }
        }
        /// <inheritdoc/>
        public long? ContentLength { get; protected set; }
        /// <inheritdoc/>
        public async Task<byte[]> ReadAsByteArrayAsync()
        {
            using Stream stream = _response?.GetResponseStream() ?? throw new InvalidOperationException("There is no content to read.");
            using MemoryStream memStream = new MemoryStream();
            await stream.CopyToAsync(memStream, int.MaxValue).ConfigureAwait(false);
            return memStream.ToArray();
        }
        /// <inheritdoc/>
        public Task<Stream> ReadAsStreamAsync()
        {
            Stream? responseStream = _response?.GetResponseStream();
            if (responseStream == null)
                return Task.FromException<Stream>(new InvalidOperationException("There is no content to read."));

            return Task.FromResult(responseStream);
        }
        /// <inheritdoc/>
        public async Task<string> ReadAsStringAsync()
        {
            using Stream stream = _response?.GetResponseStream() ?? throw new InvalidOperationException("There is no content to read.");
            using StreamReader? sr = new StreamReader(stream);
            return await sr.ReadToEndAsync().ConfigureAwait(false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_response != null)
                    {
                        // TODO: Should I be disposing of the response here? Not the content's responsibility?
                        _response.Dispose();
                        _response = null;
                    }
                }
                disposedValue = true;
            }
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
