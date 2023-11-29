using FluentValidation;
using PhoneService.Dtos.CityDtos;
using PhoneService.Entities;

namespace PhoneService.ValidationRules.CityValidatorRules
{
    public class CityCreateValidator:AbstractValidator<CityCreateDto>
    {
        public CityCreateValidator()
        {
                RuleFor(x=>x.Name).NotEmpty().WithMessage("Şehir alanı boş geçilemez");
        }
    }
}
