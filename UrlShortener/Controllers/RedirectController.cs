using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class RedirectController : Controller
    {
        private readonly IShortUrlRepository _repository;

        public RedirectController(IShortUrlRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/{code:length(7)}")]
        public async Task<IActionResult> Go(string code)
        {
            var entity = await _repository.GetByCodeAsync(code);

            if (entity is null)
            {
                return View("ShortLinkNotFound", code);
            }

            await _repository.IncrementVisitCountAsync(entity.Id);

            return Redirect(entity.OriginalUrl);
        }
    }
}