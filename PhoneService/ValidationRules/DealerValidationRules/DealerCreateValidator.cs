using FluentValidation;
using PhoneService.Dtos.DealerDtos;
using PhoneService.Entities;

namespace PhoneService.ValidationRules.DealerValidationRules
{
    public class DealerCreateValidator:AbstractValidator<DealerCreateDto>
    {
        public DealerCreateValidator()
        {
           
            RuleFor(x => x.Name).NotEmpty().WithMessage("Bayi ad alanı boş geçilemez");   

            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir  alanı boş geçilemez");   
        }
    }
}
