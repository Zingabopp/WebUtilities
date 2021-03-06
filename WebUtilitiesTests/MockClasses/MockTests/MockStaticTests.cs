using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using System;

namespace SongFeedReadersTests.MockClasses.MockTests
{
    [TestClass]
    public class MockStaticTests
    {
        [TestMethod]
        public void GetFileForUrl_BeastSaber_Bookmarked()
        {
            var testUrl = new Uri(@"https://bsaber.com/wp-json/bsaber-api/songs/?bookmarked_by=Zingabopp&page=2&count=15");
            string fileMatch = "bookmarked_by_zingabopp2.json";
            var file = new FileInfo(MockHttpResponse.GetFileForUrl(testUrl));
            Assert.AreEqual(file.Name, fileMatch);

            testUrl = new Uri(@"https://bsaber.com/wp-json/bsaber-api/songs/?bookmarked_by=Zingabopp&page=3&count=15");
            fileMatch = "bookmarked_by_zingabopp3_empty.json";
            file = new FileInfo(MockHttpResponse.GetFileForUrl(testUrl));
            Assert.AreEqual(file.Name, fileMatch);
        }

        [TestMethod]
        public void GetFileForUrl_BeastSaber_Followings()
        {
            var testUrl = new Uri(@"https://bsaber.com/members/zingabopp/wall/followings/feed/?acpage=1&count=20");
            string fileMatch = "followings1.xml";
            var file = new FileInfo(MockHttpResponse.GetFileForUrl(testUrl));
            Assert.AreEqual(file.Name, fileMatch);

            testUrl = new Uri(@"https://bsaber.com/members/zingabopp/wall/followings/feed/?acpage=8");
            fileMatch = "followings8_partial.xml";
            file = new FileInfo(MockHttpResponse.GetFileForUrl(testUrl));
            Assert.AreEqual(file.Name, fileMatch);
        }

        [TestMethod]
        public void GetFileForUrl_BeastSaber_Curator()
        {
            var testUrl = new Uri(@"https://bsaber.com/wp-json/bsaber-api/songs/?bookmarked_by=curatorrecommended&page=1&count=50");
            string fileMatch = "bookmarked_by_curator1.json";
            var file = new FileInfo(MockHttpResponse.GetFileForUrl(testUrl));
            Assert.AreEqual(file.Name, fileMatch);

            testUrl = new Uri(@"https://bsaber.com/wp-json/bsaber-api/songs/?bookmarked_by=curatorrecommended&page=4");
            fileMatch = "bookmarked_by_curator4_partial.json";
            file = new FileInfo(MockHttpResponse.GetFileForUrl(testUrl));
            Assert.AreEqual(file.Name, fileMatch);
        }

        [TestMethod]
        public void GetFileForUrl_EmptyString()
        {
            var testUrl = string.Empty;
#pragma warning disable CA2234 // Pass system uri objects instead of strings
            Assert.ThrowsException<ArgumentNullException>(() => MockHttpResponse.GetFileForUrl(testUrl));
#pragma warning restore CA2234 // Pass system uri objects instead of strings
        }

        [TestMethod]
        public void GetFileForUrl_Null_Uri()
        {
            Uri testUri = null;
            Assert.ThrowsException<ArgumentNullException>(() => MockHttpResponse.GetFileForUrl(testUri));
        }

    }
}
