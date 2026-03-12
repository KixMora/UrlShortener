using UrlShortener.Utilities;

namespace UrlShortener.Tests
{
    public class UrlValidatorTests
    {
        [Fact]
        public void TryNormalize_Returns_True_For_Valid_Https_Url()
        {
            var result = UrlValidator.TryNormalize("https://google.com", out var normalized);

            Assert.True(result);
            Assert.Equal("https://google.com/", normalized);
        }

        [Fact]
        public void TryNormalize_Returns_False_For_Invalid_Text()
        {
            var result = UrlValidator.TryNormalize("abc123", out var normalized);

            Assert.False(result);
            Assert.Equal(string.Empty, normalized);
        }
    }
}