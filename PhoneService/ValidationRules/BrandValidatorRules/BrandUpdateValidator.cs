using FluentValidation;
using PhoneService.Dtos.BrandDtos;

namespace PhoneService.ValidationRules.BrandValidatorRules
{
    public class BrandUpdateValidator:AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Marka alanı boş geçilemez");
        }
    }
}
