using FluentValidation;
using FreeCourse.Frontends.Web.Models.Catalogs;

namespace FreeCourse.Frontends.Web.Validators.Courses
{
    public class CourseUpdateInputValidator: AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("Hatalı para formatı");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz");
        }
    }
}
