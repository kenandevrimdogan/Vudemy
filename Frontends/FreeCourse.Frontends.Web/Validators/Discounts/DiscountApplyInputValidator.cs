using FluentValidation;
using FreeCourse.Frontends.Web.Models.Discounts;

namespace FreeCourse.Frontends.Web.Validators.Discounts
{
    public class DiscountApplyInputValidator: AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("İndirim Kodu Boş Geçilemez");
        }
    }
}
