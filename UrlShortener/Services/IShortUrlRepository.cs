using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IShortUrlRepository
    {
        Task<List<ShortUrl>> GetAllAsync();
        Task<ShortUrl?> GetByIdAsync(int id);
        Task<ShortUrl?> GetByCodeAsync(string code);
        Task<bool> ShortCodeExistsAsync(string code);
        Task<int> CreateAsync(ShortUrl shortUrl);
        Task UpdateAsync(ShortUrl shortUrl);
        Task DeleteAsync(int id);
        Task IncrementVisitCountAsync(int id);
    }
}