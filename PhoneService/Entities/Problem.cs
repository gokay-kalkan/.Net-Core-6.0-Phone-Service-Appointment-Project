using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class Problem
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        public virtual List<Price> Prices { get; set; }
    }
}
