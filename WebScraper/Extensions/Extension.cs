using System.IO;
using System.Net.Http;
using WebScraper.Model;

namespace WebScraper.Extentions
{
    public static class Extension
    {
        /// <summary>
        /// Downloads an image from the specified URL and stores the image data in the `Image` property of the provided SkinCase object.
        /// </summary>
        /// <param name="case">The SkinCase object for which to download the image.</param>
        public static void DownloadImage(this SkinCase @case)
        {
            using (HttpClient client = new())
            {
                var y = client.GetAsync(@case.ImageUrl).Result;
                using (Stream m = y.Content.ReadAsStream())
                {
                    using (MemoryStream s = new())
                    {
                        m.CopyTo(s);
                        @case.Image = s.ToArray();
                    }
                }

            }
        }
    }
}
