using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.Net;
using WebScraper;
namespace NUnitTests

{
    public class WebScraperTests
    {
        // private string cssourcecode = null;

        [SetUp]
        public void Setup()
        {
            // Testfall for Extrackskin
            //Assembly a = typeof(WebScraperTests).Assembly;
            //using Stream y = a.GetManifestResourceStream("NUnitTests.TestFiles.CSSourcecode.htm");
            //using StreamReader s = new(y);
            //cssourcecode = s.ReadToEnd();
            //var b = WebScraperClass.ExtractSkinCases(cssourcecode);
            //b[0].DownloadImage();
        }

        [Test]
        public void DownloadSourceCodeTest()
        {
            MockHttpMessageHandler mockHttp = new();
            mockHttp.When("http://foobar.com/*")
                    .Respond("text/html", "Test HTML Response");
            WebScraperClass wsc = new()
            {
                client = mockHttp.ToHttpClient(),
                alteranativeUrl = "http://foobar.com/"
            };
            string result = null;
            Assert.DoesNotThrowAsync(async () => result = await wsc.DownloadSourceCode());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("Test HTML Response"));

        }

        [Test]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.NoContent)]
        [TestCase(HttpStatusCode.Unauthorized)]
        public void DownloadSourceCodeFailerTest(HttpStatusCode status)
        {
            MockHttpMessageHandler mockHttp = new();
            mockHttp.When("http://foobar.com/*")
                    .Respond(status);
            WebScraperClass wsc = new()
            {
                client = mockHttp.ToHttpClient(),
                alteranativeUrl = "http://foobar.com/"
            };
            string result = null;
            Assert.DoesNotThrowAsync(async () => result = await wsc.DownloadSourceCode());
            Assert.That(result, Is.Null);
        }

    }
}