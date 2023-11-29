using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class Model
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public bool Status { get; set; }

        public virtual List<Appointment> Appointments { get; set; }
    }
}
