using System.ComponentModel.DataAnnotations;

namespace UrlShortener.ViewModels
{
    public class EditShortUrlVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите ссылку.")]
        [MaxLength(2048, ErrorMessage = "Ссылка слишком длинная.")]
        [Display(Name = "Длинный URL")]
        public string OriginalUrl { get; set; } = string.Empty;

        public string ShortCode { get; set; } = string.Empty;

        public long VisitCount { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}