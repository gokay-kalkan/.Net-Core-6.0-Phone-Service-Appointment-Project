using FluentValidation;
using PhoneService.Dtos.PriceDtos;
using PhoneService.Entities;

namespace PhoneService.ValidationRules.PriceValidatorRules
{
    public class PriceCreateValidator:AbstractValidator<PriceCreateDto>
    {
        public PriceCreateValidator()
        {
            RuleFor(x => x.PriceValue).NotEmpty().WithMessage("Fiyat değeri boş geçilemez");

            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir alanı boş geçilemez");

            RuleFor(x => x.DealerId).NotEmpty().WithMessage("Bayi boş geçilemez");
        }
    }
}
