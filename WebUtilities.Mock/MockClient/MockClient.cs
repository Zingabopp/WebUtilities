using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebUtilities.Mock.MockClient
{
    /// <summary>
    /// A mock IWebClient implementation
    /// </summary>
    public class MockClient : IWebClient
    {
        /// <summary>
        /// Used for recording mode, requests will be passed to this client and the responses saved to disk.
        /// </summary>
        private IWebClient? _recordingClient;
        /// <summary>
        /// Directory to record responses to
        /// </summary>
        private string? _responsePath;
        /// <summary>
        /// Converts a Uri to a filename.
        /// </summary>
        private Func<Uri, string> _filenameFactory;
        /// <summary>
        /// Maps URLs without wildcards to file paths.
        /// </summary>
        protected Dictionary<string, string> exactUrlMap = new Dictionary<string, string>();
        /// <summary>
        /// Maps URLs with wildcards to file paths.
        /// </summary>
        protected List<(string url, string path)> wildCardMap = new List<(string url, string path)>();
        /// <inheritdoc/>
        public int Timeout
        {
            get;
            set;
        }
        private string? _userAgent;
        /// <inheritdoc/>
        public string? UserAgent
        {
            get => _userAgent;
            set
            {
                _userAgent = value;
                _recordingClient?.SetUserAgent(value);
            }
        }

        /// <inheritdoc/>
        public ErrorHandling ErrorHandling { get; set; }


        /// <inheritdoc/>
        public void SetUserAgent(string? userAgent)
        {
            UserAgent = userAgent;
        }
        /// <summary>
        /// Creates a new <see cref="MockClient"/>.
        /// </summary>
        /// <param name="responsePath"></param>
        /// <param name="filenameFactory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public MockClient(string responsePath, Func<Uri, string> filenameFactory)
        {
            _responsePath = responsePath ?? throw new ArgumentNullException(nameof(responsePath));
            if (!Directory.Exists(responsePath))
                throw new DirectoryNotFoundException($"Directory '{responsePath}' does not exist.");
            _filenameFactory = filenameFactory ?? throw new ArgumentNullException(nameof(filenameFactory));
        }

        /// <summary>
        /// Records responses from <paramref name="client"/> and saves them to the the response path.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public MockClient WithRecordingClient(IWebClient client)
        {
            _recordingClient = client ?? throw new ArgumentNullException(nameof(client));
            return this;
        }

        /// <inheritdoc/>
        public async Task<IWebResponseMessage> GetAsync(Uri uri, int timeout, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_responsePath, _filenameFactory(uri));
            string responseDataPath = path + ".mock";
            MockResponseData? responseData = null;
            Exception? exception = null;
            try
            {
                if (_recordingClient != null)
                {
                    IWebResponseMessage? response = await _recordingClient.GetAsync(uri, timeout, cancellationToken);
                    responseData = new MockResponseData(response);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    await File.WriteAllTextAsync(responseDataPath, JsonConvert.SerializeObject(responseData, Formatting.Indented));
                    if (response.Content != null)
                        await response.Content.ReadAsFileAsync(path, true, CancellationToken.None);
                }
                else
                {
                    if (!File.Exists(responseDataPath))
                    {
                        throw new FileNotFoundException($"Response data not found at '{path}'");
                    }
                    string data = await File.ReadAllTextAsync(responseDataPath, cancellationToken).ConfigureAwait(false);
                    responseData = JsonConvert.DeserializeObject<MockResponseData>(data)
                        ?? throw new Exception("Failed to deserialize response data.");

                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return new MockResponse(path, responseData, uri, exception);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
