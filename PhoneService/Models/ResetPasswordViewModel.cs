using System.ComponentModel.DataAnnotations;

namespace PhoneService.Models
{
    public class ResetPasswordViewModel
    {
        

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Onay")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifre ve şifre onayı eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; } // Şifre sıfırlama token'i
    }
}
