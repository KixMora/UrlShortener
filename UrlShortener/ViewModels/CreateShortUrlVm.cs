using System.ComponentModel.DataAnnotations;

namespace UrlShortener.ViewModels
{
    public class CreateShortUrlVm
    {
        [Required(ErrorMessage = "Введите ссылку.")]
        [MaxLength(2048, ErrorMessage = "Ссылка слишком длинная.")]
        [Display(Name = "Длинный URL")]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}