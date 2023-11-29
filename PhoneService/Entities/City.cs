using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Status { get; set; }
        public virtual List<Dealer> Dealers { get; set; }
        public virtual List<Price> Prices { get; set; }

        public virtual List<Appointment> Appointments { get; set; }
    }
}
