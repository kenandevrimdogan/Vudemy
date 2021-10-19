using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Frontends.Web.Models.Catalogs
{
    public class CourseCreateInput
    {
        [Display(Name = "Kurs İsmi", Prompt = "Kurs İsmi")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Kurs Fiyat", Prompt = "Kurs Fiyat")]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Kurs Açıklama", Prompt = "Kurs Açıklama")]
        [Required]
        public string Description { get; set; }

        public string UserId { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs Kategori", Prompt = "Kurs Kategori")]
        [Required]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs Resmi", Prompt = "Kurs Resmi")]
        public IFormFile PhotoFormFile { get; set; }

        public string Picture { get; set; }
    }
}
