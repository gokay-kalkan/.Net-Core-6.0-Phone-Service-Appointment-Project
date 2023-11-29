using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
    
        public string Email { get; set; }
        public string Password { get; set; }

        public bool Approved { get; set; }

        public string? ResetPasswordToken { get; set; }

        
        public DateTime? ResetPasswordExpiration { get; set; }
    }
}
