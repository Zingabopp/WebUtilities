using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WebUtilities.WebWrapper;

namespace WebUtilitiesTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            //Uri uri = new Uri("http://releases.ubuntu.com/18.04.3/ubuntu-18.04.3-desktop-amd64.iso");
            //var client = new WebClientWrapper();
            //var response = await client.GetAsync(uri);
            //var stream = await response.Content.ReadAsStreamAsync();
            //using var mem = new MemoryStream();
            //await stream.CopyToAsync(mem);

            await Task.Yield();
            string thing = Path.GetFullPath("MockClasses");
            string thing2 = Path.GetFullPath(@"MockClasses\");
            DirectoryInfo thing3 = new DirectoryInfo(@"MockClasses");
            DirectoryInfo thing4 = new DirectoryInfo(@"MockClasses\");
            DirectoryInfo[] thing5 = thing3.Parent.GetDirectories();
        }


    }
}
