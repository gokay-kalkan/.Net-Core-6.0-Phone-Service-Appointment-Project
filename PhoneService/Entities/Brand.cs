using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Status { get; set; }
        public virtual List<Model> Models { get; set; }

        public virtual List<Appointment> Appointments { get; set; }
    }
}
