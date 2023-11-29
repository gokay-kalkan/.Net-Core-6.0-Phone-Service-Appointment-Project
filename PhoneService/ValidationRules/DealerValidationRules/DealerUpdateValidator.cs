using FluentValidation;
using PhoneService.Dtos.DealerDtos;

namespace PhoneService.ValidationRules.DealerValidationRules
{
    public class DealerUpdateValidator:AbstractValidator<DealerUpdateDto>
    {
        public DealerUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Bayi ad alanı boş geçilemez");

            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir  alanı boş geçilemez");
        }
    }
}
