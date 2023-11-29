using FluentValidation;
using PhoneService.Dtos.ModelDtos;

namespace PhoneService.ValidationRules.ModelValidationRules
{
    public class ModelUpdateValdiator : AbstractValidator<ModelUpdateDto>
    {
        public ModelUpdateValdiator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Model alanı boş geçilemez");

            RuleFor(x => x.BrandId).NotEmpty().WithMessage("Marka alanı boş geçilemez");
        }
    }
}
