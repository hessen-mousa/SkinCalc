namespace WebScraper.Model
{
    public record SkinCase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }

    }
}
