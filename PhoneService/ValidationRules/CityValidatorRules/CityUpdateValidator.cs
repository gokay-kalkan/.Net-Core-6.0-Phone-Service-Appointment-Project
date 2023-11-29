using FluentValidation;
using PhoneService.Dtos.CityDtos;

namespace PhoneService.ValidationRules.CityValidatorRules
{
    public class CityUpdateValidator:AbstractValidator<CityUpdateDto>
    {
        public CityUpdateValidator() 
        { 
        
        }    
    }
}
