using MySqlConnector;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly string _connectionString;

        public ShortUrlRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
        }

        public async Task<List<ShortUrl>> GetAllAsync()
        {
            var items = new List<ShortUrl>();

            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                SELECT Id, OriginalUrl, ShortCode, CreatedAtUtc, VisitCount
                FROM ShortUrls
                ORDER BY CreatedAtUtc DESC;
                """;

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(Map(reader));
            }

            return items;
        }

        public async Task<ShortUrl?> GetByIdAsync(int id)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                SELECT Id, OriginalUrl, ShortCode, CreatedAtUtc, VisitCount
                FROM ShortUrls
                WHERE Id = @Id;
                """;

            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return Map(reader);
            }

            return null;
        }

        public async Task<ShortUrl?> GetByCodeAsync(string code)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                SELECT Id, OriginalUrl, ShortCode, CreatedAtUtc, VisitCount
                FROM ShortUrls
                WHERE ShortCode = @Code;
                """;

            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Code", code);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return Map(reader);
            }

            return null;
        }

        public async Task<bool> ShortCodeExistsAsync(string code)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                SELECT COUNT(*)
                FROM ShortUrls
                WHERE ShortCode = @Code;
                """;

            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Code", code);

            var count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<int> CreateAsync(ShortUrl shortUrl)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                INSERT INTO ShortUrls (OriginalUrl, ShortCode, CreatedAtUtc, VisitCount)
                VALUES (@OriginalUrl, @ShortCode, @CreatedAtUtc, @VisitCount);
                SELECT LAST_INSERT_ID();
                """;

            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OriginalUrl", shortUrl.OriginalUrl);
            command.Parameters.AddWithValue("@ShortCode", shortUrl.ShortCode);
            command.Parameters.AddWithValue("@CreatedAtUtc", shortUrl.CreatedAtUtc);
            command.Parameters.AddWithValue("@VisitCount", shortUrl.VisitCount);

            var id = Convert.ToInt32(await command.ExecuteScalarAsync());
            return id;
        }

        public async Task UpdateAsync(ShortUrl shortUrl)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                UPDATE ShortUrls
                SET OriginalUrl = @OriginalUrl
                WHERE Id = @Id;
                """;

            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OriginalUrl", shortUrl.OriginalUrl);
            command.Parameters.AddWithValue("@Id", shortUrl.Id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                DELETE FROM ShortUrls
                WHERE Id = @Id;
                """;

            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task IncrementVisitCountAsync(int id)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = """
                UPDATE ShortUrls
                SET VisitCount = VisitCount + 1
                WHERE Id = @Id;
                """;

            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private static ShortUrl Map(MySqlDataReader reader)
        {
            return new ShortUrl
            {
                Id = reader.GetInt32("Id"),
                OriginalUrl = reader.GetString("OriginalUrl"),
                ShortCode = reader.GetString("ShortCode"),
                CreatedAtUtc = reader.GetDateTime("CreatedAtUtc"),
                VisitCount = reader.GetInt64("VisitCount")
            };
        }
    }
}