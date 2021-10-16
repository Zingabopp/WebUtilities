using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUtilities.Mock.MockClient
{
    public class MockContent : IWebResponseContent
    {
        private string _contentPath;
        private string? contentTypeOverride;
        private long _contentLengthOverride = -1;
        public string? ContentType
        {
            get
            {
                if (contentTypeOverride != null)
                    return contentTypeOverride;
                if (Headers.TryGetValue("content-type", out var values))
                {
                    return values.FirstOrDefault();
                }
                return string.Empty;
            }
        }

        public long? ContentLength
        {
            get
            {
                if (_contentLengthOverride >= 0)
                    return _contentLengthOverride;
                if (Headers.TryGetValue("content-length", out var values))
                {
                    string value = values.FirstOrDefault();
                    if(long.TryParse(value, out long result))
                    {
                        return result;
                    }
                }
                return 0;
            }
        }

        public ReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

        public Task<byte[]> ReadAsByteArrayAsync()
        {
            return File.ReadAllBytesAsync(_contentPath);
        }

        public Task<Stream> ReadAsStreamAsync()
        {
            FileStream fs = File.OpenRead(_contentPath);
            return Task.FromResult<Stream>(fs);
        }

        public Task<string> ReadAsStringAsync()
        {
            return File.ReadAllTextAsync(_contentPath);
        }

        public MockContent(string path, MockResponseData responseData)
        {
            _contentPath = path ?? throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                throw new FileNotFoundException($"Content file at '{path}' not found.");
            Headers = new ReadOnlyDictionary<string, IEnumerable<string>>(responseData.ContentHeaders);
        }

        public void Dispose()
        {
        }
    }
}
