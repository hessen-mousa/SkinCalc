using System.IO;
using System.Net.Http;
using WebScraper.Model;

namespace WebScraper.Extentions
{
    public static class Extension
    {
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
