using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class Dealer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }


        public int CityId { get; set; }

        public virtual City City { get; set; }

        public bool Status { get; set; }

        public virtual List<Price> Prices { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
    }
}
