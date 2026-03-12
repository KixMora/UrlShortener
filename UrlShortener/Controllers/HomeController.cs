using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.ViewModels;
using UrlShortener.Utilities;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShortUrlRepository _repository;
        private readonly IShortCodeGenerator _codeGenerator;

        public HomeController(IShortUrlRepository repository, IShortCodeGenerator codeGenerator)
        {
            _repository = repository;
            _codeGenerator = codeGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new HomeIndexVm
            {
                Items = await _repository.GetAllAsync(),
                BaseUrl = $"{Request.Scheme}://{Request.Host}/"
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "CreateForm")] CreateShortUrlVm vm)
        {
            if (!UrlValidator.TryNormalize(vm.OriginalUrl, out var normalizedUrl))
            {
                ModelState.AddModelError(nameof(vm.OriginalUrl), "Введите корректный URL с http:// или https://");
            }

            if (!ModelState.IsValid)
            {
                var invalidVm = new HomeIndexVm
                {
                    CreateForm = vm,
                    Items = await _repository.GetAllAsync(),
                    BaseUrl = $"{Request.Scheme}://{Request.Host}/"
                };

                return View("Index", invalidVm);
            }

            string shortCode;
            do
            {
                shortCode = _codeGenerator.Generate(7);
            }
            while (await _repository.ShortCodeExistsAsync(shortCode));

            var entity = new ShortUrl
            {
                OriginalUrl = normalizedUrl,
                ShortCode = shortCode,
                CreatedAtUtc = DateTime.UtcNow,
                VisitCount = 0
            };

            await _repository.CreateAsync(entity);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null)
            {
                return NotFound();
            }

            var vm = new EditShortUrlVm
            {
                Id = entity.Id,
                OriginalUrl = entity.OriginalUrl,
                ShortCode = entity.ShortCode,
                VisitCount = entity.VisitCount,
                CreatedAtUtc = entity.CreatedAtUtc
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditShortUrlVm vm)
        {
            if (!UrlValidator.TryNormalize(vm.OriginalUrl, out var normalizedUrl))
            {
                ModelState.AddModelError(nameof(vm.OriginalUrl), "Введите корректный URL с http:// или https://");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var entity = await _repository.GetByIdAsync(vm.Id);
            if (entity is null)
            {
                return NotFound();
            }

            entity.OriginalUrl = normalizedUrl;

            await _repository.UpdateAsync(entity);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}