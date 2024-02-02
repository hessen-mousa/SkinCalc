using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Runtime.Intrinsics.X86;
using System.Net;

namespace WebScraper
{
    public class WebScraperClass : IDisposable
    {
        private const string URL = "https://csgostash.com/containers/skin-cases?name=&sort=name&order=asc";
        internal string alteranativeUrl = null; 
        internal HttpClient client = null;
        private bool disposedValue;

        /// <summary>
        /// Asynchronously downloads the source code of a web page. The web page URL is determined by the alternativeUrl field;
        /// if alternativeUrl is null, the default URL is used. Returns the source code as a string, or null if the download fails.
        /// </summary>
        /// <returns>A  <see cref="Task"/> that represents the asynchronous download operation. The Task result contains the web page source code as a string, or null if an error occurs.</returns>
        internal async Task<string> DownloadSourceCode()
        {
            this.client ??= new();
            try
            {
                using HttpResponseMessage response = await client.GetAsync(alteranativeUrl ?? URL);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.client?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
