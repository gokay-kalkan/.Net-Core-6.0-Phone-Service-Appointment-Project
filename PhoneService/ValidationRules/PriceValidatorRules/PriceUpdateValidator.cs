using FluentValidation;
using PhoneService.Dtos.PriceDtos;

namespace PhoneService.ValidationRules.PriceValidatorRules
{
    public class PriceUpdateValidator : AbstractValidator<PriceUpdateDto>
    {
        public PriceUpdateValidator()

        {
            RuleFor(x => x.PriceValue).NotEmpty().WithMessage("Fiyat değeri boş geçilemez");

            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir alanı boş geçilemez");

            RuleFor(x => x.DealerId).NotEmpty().WithMessage("Bayi boş geçilemez");
        }
    }
}

