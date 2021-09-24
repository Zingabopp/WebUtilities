using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebUtilities;
using WebUtilities.HttpClientWrapper;
using WebUtilities.WebWrapper;

namespace WebUtilitiesTests.HttpClientWrapperTests
{
    [TestClass]
    public class WebClientContent_Tests
    {
        private static readonly string TestOutputPath = Path.GetFullPath(@"Output\WebClientContent_Tests");

        [TestMethod]
        public async Task CanceledDownload()
        {
            IWebClient client = new HttpClientWrapper();
            Uri uri = new Uri("http://releases.ubuntu.com/18.04.3/ubuntu-18.04.3-desktop-amd64.iso");
            CancellationTokenSource cts = new CancellationTokenSource(700);
            Directory.CreateDirectory(TestOutputPath);
            string filePath = Path.Combine(TestOutputPath, "CanceledDownload.iso");

            using (IWebResponseMessage response = client.GetAsync(uri).Result)
            {
                if (!response.IsSuccessStatusCode)
                    Assert.Fail($"Error getting response from {uri}: {response.ReasonPhrase}");
                try
                {
                    CancellationTokenSource readCts = new CancellationTokenSource(100);
                    string thing = await response.Content.ReadAsFileAsync(filePath, true, readCts.Token);
                    Assert.Fail("Didn't throw exception");
                }
                catch (OperationCanceledException)
                {
                    // Success
                }
                catch (Exception)
                {
                    Assert.Fail("Wrong exception thrown.");
                }
            }
            cts.Dispose();
            client.Dispose();
            await Task.Yield();
        }

        [TestMethod]
        public async Task CanceledBeforeResponse()
        {
            IWebClient client = new HttpClientWrapper("WebUtilitiesTests/1.0.0");
            Uri uri = new Uri("http://releases.ubuntu.com/18.04.3/ubuntu-18.04.3-desktop-amd64.iso");
            CancellationTokenSource cts = new CancellationTokenSource(100);
            Directory.CreateDirectory(TestOutputPath);
            string filePath = Path.Combine(TestOutputPath, "CanceledBeforeResponse.iso");
            try
            {
                using IWebResponseMessage response = await client.GetAsync(uri, cts.Token);
                if (!response.IsSuccessStatusCode)
                    Assert.Fail($"Error getting response from {uri}: {response.ReasonPhrase}");
                Assert.Fail("Didn't throw exception");
            }
            catch (OperationCanceledException)
            {
                // Success
            }
            catch (Exception)
            {
                Assert.Fail("Wrong exception thrown.");
            }
            cts.Dispose();
            client.Dispose();
        }
    }
}
