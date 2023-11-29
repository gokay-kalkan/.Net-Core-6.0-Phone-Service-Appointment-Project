using FluentValidation;
using PhoneService.Entities;

namespace PhoneService.ValidationRules.AppointmentValidatorRules
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
           
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Müşteri ad alanı boş geçilemez");

            RuleFor(x => x.CustomerSurname).NotEmpty().WithMessage("Müşteri soyad alanı boş geçilemez");

            RuleFor(x => x.CustomerEmail).NotEmpty().WithMessage("Müşteri email alanı boş geçilemez");

            RuleFor(x => x.CustomerPhone).NotEmpty().WithMessage("Müşteri telefon alanı boş geçilemez");

            RuleFor(x => x.BrandId).NotEmpty().WithMessage("Marka alanı boş geçilemez");

            RuleFor(x => x.ModelId).NotEmpty().WithMessage("Model alanı boş geçilemez");

            RuleFor(x => x.CityId).NotEmpty().WithMessage("Şehir alanı boş geçilemez");

            RuleFor(x => x.DealerId).NotEmpty().WithMessage("Bayi alanı boş geçilemez");

        }
    }
}
