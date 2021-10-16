using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace WebUtilities.Mock.MockClient
{
    public class MockResponseData
    {
        [JsonProperty("ResponseHeaders", DefaultValueHandling = DefaultValueHandling.Populate)]
        public Dictionary<string, IEnumerable<string>> ResponseHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();
        [JsonProperty("ContentHeaders", DefaultValueHandling = DefaultValueHandling.Populate)]
        public Dictionary<string, IEnumerable<string>> ContentHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();
        [JsonProperty("StatusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("ContentType")]
        public string? ContentTypeOverride { get; set; }
        [JsonProperty("ContentLengthOverride", DefaultValueHandling = DefaultValueHandling.Populate)]
        public long ContentLengthOverride { get; set; } = -1;

        public MockResponseData() { }

        public MockResponseData(IWebResponseMessage webResponse)
        {
            StatusCode = webResponse.StatusCode;
            ResponseHeaders = new Dictionary<string, IEnumerable<string>>(webResponse.Headers);

            IWebResponseContent? content = webResponse.Content;
            if (content != null)
                ContentHeaders = new Dictionary<string, IEnumerable<string>>(content.Headers);
        }

    }
}
