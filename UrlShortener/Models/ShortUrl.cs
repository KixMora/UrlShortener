namespace UrlShortener.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
        public long VisitCount { get; set; }
    }
}