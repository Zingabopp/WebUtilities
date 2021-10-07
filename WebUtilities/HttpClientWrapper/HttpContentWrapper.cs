using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebUtilities.HttpClientWrapper
{
    /// <summary>
    /// Wrapper for the content from a <see cref="HttpResponseWrapper"/>.
    /// </summary>
    public class HttpContentWrapper : IWebResponseContent
    {
        private HttpContent? _content;

        /// <summary>
        /// Creates a new <see cref="HttpContentWrapper"/> from the provided <paramref name="content"/>.
        /// </summary>
        /// <param name="content"></param>
        public HttpContentWrapper(HttpContent? content)
        {
            _content = content;
            _headers = new Dictionary<string, IEnumerable<string>>();
            if (_content?.Headers != null)
            {
                foreach (KeyValuePair<string, IEnumerable<string>> header in _content.Headers)
                {
                    _headers.Add(header.Key, header.Value);
                }
            }
            try
            {
                ContentLength = content?.Headers?.ContentLength;
            }
            catch (ObjectDisposedException)
            {
                _content = null;
            }

        }
        /// <summary>
        /// Response headers.
        /// </summary>
        protected Dictionary<string, IEnumerable<string>> _headers;
        /// <inheritdoc/>
        public ReadOnlyDictionary<string, IEnumerable<string>> Headers
        {
            get { return new ReadOnlyDictionary<string, IEnumerable<string>>(_headers); }
        }

        /// <inheritdoc/>
        public string ContentType { get { return _content?.Headers?.ContentType?.MediaType ?? string.Empty; } }

        /// <inheritdoc/>
        public long? ContentLength { get; protected set; }

        /// <inheritdoc/>
        public Task<byte[]> ReadAsByteArrayAsync()
        {
            HttpContent? content = _content;
            if (content == null)
                return Task.FromException<byte[]>(new InvalidOperationException("There is no content to read."));
            return content.ReadAsByteArrayAsync();
        }

        /// <inheritdoc/>
        public Task<Stream> ReadAsStreamAsync()
        {
            HttpContent? content = _content;
            if (content == null)
                return Task.FromException<Stream>(new InvalidOperationException("There is no content to read."));
            return content.ReadAsStreamAsync();
        }

        /// <inheritdoc/>
        public Task<string> ReadAsStringAsync()
        {
            HttpContent? content = _content;
            if (content == null)
                return Task.FromException<string>(new InvalidOperationException("There is no content to read."));
            return content.ReadAsStringAsync();
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
                    if (_content != null)
                    {
                        _content.Dispose();
                        _content = null;
                        ContentLength = null;
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
