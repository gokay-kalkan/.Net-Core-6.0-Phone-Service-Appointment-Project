using FluentValidation;
using PhoneService.Dtos.ModelDtos;
using PhoneService.Entities;

namespace PhoneService.ValidationRules.ModelValidationRules
{
    public class ModelCreateValidator:AbstractValidator<ModelCreateDto>
    {
        public ModelCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Model alanı boş geçilemez");

            RuleFor(x => x.BrandId).NotEmpty().WithMessage("Marka alanı boş geçilemez");
        }
    }
}
