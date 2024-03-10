using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebScraper.Model;

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

        /// <summary>
        /// Parses a given HTML string to extract information about skin cases that include "Case" in their names,
        /// specifically targeting div elements with a class of 'well result-box nomargin'.
        /// Each skin case's information includes the name found within an <h4> element, the price formatted as a decimal located within a paragraph element
        /// having a 'nomargin' class inside a div with a 'price' class, and the image URL from the src attribute of an <img> element.
        /// This method ensures that only skin cases with "Case" in the <h4> tag name are extracted and converts the price text to a decimal for precise financial representation.
        /// </summary>
        /// <param name="html">The HTML string to be parsed for skin case information.</param>
        /// <returns>A <see cref="IEnumerable{SkinCase}"/> of <see cref="SkinCase"/> objects, each representing a skin case with its name, precise decimal price, and image URL extracted from the HTML.
        /// Only skin cases with "Case" in their names are included. Returns an empty list if no matching elements are found.</returns>
        public async Task<IEnumerable<SkinCase>> ExtractSkinCases()
        {
            string html = await DownloadSourceCode();
            var skinCases = new List<SkinCase>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Selects all 'div' elements with the class 'well result-box nomargin'
            var caseNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'well result-box nomargin')]");

            if (caseNodes != null)
            {
                foreach (var node in caseNodes)
                {
                    var h4Node = node.SelectSingleNode(".//h4");
                    // Ensure the <h4> element contains "Case" in its name
                    if (h4Node != null && h4Node.InnerText.Contains("Case"))
                    {
                        var priceNode = node.SelectSingleNode(".//div[contains(@class, 'price')]/p[@class='nomargin']");
                        decimal price = 0m;
                        if (priceNode != null)
                        {
                            // Extract the price as text, remove any currency symbols and replace commas with dots
                            var priceText = priceNode.InnerText.Trim().Replace("€", "").Replace(",", ".");
                            // Try to parse the text as a decimal
                            decimal.TryParse(priceText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out price);
                        }

                        var skinCase = new SkinCase
                        {
                            Name = h4Node.InnerText.Trim(),
                            Price = price,
                            ImageUrl = node.SelectSingleNode(".//img")?.Attributes["src"]?.Value
                        };

                        if (skinCase.Name != null && skinCase.ImageUrl != null)
                        {
                            skinCases.Add(skinCase);
                        }
                    }
                }
            }

            return skinCases;
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
