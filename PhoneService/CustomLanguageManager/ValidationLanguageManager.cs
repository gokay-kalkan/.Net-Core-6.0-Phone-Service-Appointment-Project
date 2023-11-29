using FluentValidation;
using FluentValidation.Resources;

namespace PhoneService.CustomLanguageManager
{
    public class ValidationLanguageManager : LanguageManager
    {
        public ValidationLanguageManager()
        {
            AddTranslation("tr-TR", "NotEmptyValidator", "'{PropertyName}' alanı boş geçilemez.");

        }
        


    }
}

