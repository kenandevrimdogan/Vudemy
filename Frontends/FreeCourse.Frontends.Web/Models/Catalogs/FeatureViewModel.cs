using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Frontends.Web.Models.Catalogs
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs Süre", Prompt = "Kurs Süre")]
        public int Duration { get; set; }
    }
}
