using UrlShortener.Models;

namespace UrlShortener.ViewModels
{
    public class HomeIndexVm
    {
        public CreateShortUrlVm CreateForm { get; set; } = new();
        public List<ShortUrl> Items { get; set; } = new();
        public string BaseUrl { get; set; } = string.Empty;
    }
}