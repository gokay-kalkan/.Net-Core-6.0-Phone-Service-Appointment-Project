using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
      

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string AppointmentNumber { get; set; }

        public decimal Price { get; set; }

        public bool Status { get; set; }
        public int DealerId { get; set; }
        public virtual Dealer Dealer { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public int ModelId { get; set; }

        public virtual Model Model { get; set; }

        public int ProblemId { get; set; }

        public virtual Problem Problem { get; set; }
    }
}
