namespace UrlShortener.Utilities
{
    public static class UrlValidator
    {
        public static bool TryNormalize(string input, out string normalizedUrl)
        {
            normalizedUrl = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            if (!Uri.TryCreate(input.Trim(), UriKind.Absolute, out var uri))
            {
                return false;
            }

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            normalizedUrl = uri.ToString();
            return true;
        }
    }
}