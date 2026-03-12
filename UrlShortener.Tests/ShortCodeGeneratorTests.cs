using UrlShortener.Services;

namespace UrlShortener.Tests
{
    public class ShortCodeGeneratorTests
    {
        [Fact]
        public void Generate_Returns_Seven_Characters_From_Allowed_Set()
        {
            var generator = new ShortCodeGenerator();

            var code = generator.Generate(7);

            Assert.Equal(7, code.Length);
            Assert.Matches("^[a-zA-Z0-9]{7}$", code);
        }
    }
}