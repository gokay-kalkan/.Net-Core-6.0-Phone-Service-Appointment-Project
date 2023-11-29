using FluentValidation;
using PhoneService.Dtos.BrandDtos;
using PhoneService.Entities;

namespace PhoneService.ValidationRules.BrandValidatorRules
{
    public class BrandCreateValidator : AbstractValidator<BrandCreateDto>
    {
        public BrandCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Marka alanı boş geçilemez");
        }
    }
}
