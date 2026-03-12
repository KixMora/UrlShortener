using System.Security.Cryptography;
using System.Text;

namespace UrlShortener.Services
{
    public class ShortCodeGenerator : IShortCodeGenerator
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string Generate(int length = 7)
        {
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(AllowedChars.Length);
                result.Append(AllowedChars[index]);
            }

            return result.ToString();
        }
    }
}