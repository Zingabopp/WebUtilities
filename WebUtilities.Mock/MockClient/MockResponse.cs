using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WebUtilities.Mock.MockClient
{
    public class MockResponse : IWebResponseMessage
    {
        /// <inheritdoc/>
        public int StatusCode { get; }

        /// <inheritdoc/>
        public string? ReasonPhrase { get; }

        /// <inheritdoc/>
        public Exception? Exception { get; }
        /// <inheritdoc/>
        public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode < 300;

        /// <inheritdoc/>
        public Uri RequestUri { get; }

        private readonly Dictionary<string, IEnumerable<string>> _headers = new Dictionary<string, IEnumerable<string>>();
        /// <inheritdoc/>
        public ReadOnlyDictionary<string, IEnumerable<string>> Headers
            => new ReadOnlyDictionary<string, IEnumerable<string>>(_headers);

        public IWebResponseContent? Content { get; }


        public IWebResponseMessage EnsureSuccessStatusCode()
        {
            if (!IsSuccessStatusCode)
            {
                throw new WebClientException($"The remove server returned an error: ({StatusCode}) {ReasonPhrase}.");
            }
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="MockResponse"/> from the given <paramref name="mockDataPath"/>.
        /// </summary>
        /// <param name="mockDataPath"></param>
        /// <param name="requestUri"></param>
        /// <param name="exception"></param>
        public MockResponse(string? mockDataPath, MockResponseData? responseData, Uri requestUri, Exception? exception = null)
        {
            RequestUri = requestUri;

            if (responseData != null)
            {
                StatusCode = responseData.StatusCode;
                Content = new MockContent(mockDataPath, responseData);
            }
            Exception = exception;
            if(exception == null)
            {
                if (mockDataPath == null)
                    throw new ArgumentNullException(nameof(mockDataPath));
                if (responseData == null)
                    throw new ArgumentNullException(nameof(responseData));
                _headers = responseData.ResponseHeaders;
            }
        }

        public void Dispose()
        {
        }
    }
}
