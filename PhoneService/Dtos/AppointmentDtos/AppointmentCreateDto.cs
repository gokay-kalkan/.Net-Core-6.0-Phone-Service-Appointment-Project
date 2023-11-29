using PhoneService.Entities;
using System.ComponentModel.DataAnnotations;

namespace PhoneService.Dtos.AppointmentDtos
{
    public class AppointmentCreateDto
    {
        public string ProblemDetail { get; set; }

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        public string AppointmentTime { get; set; }
        public int DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        public int CityId { get; set; }

        public int BrandId { get; set; }

        public int ModelId { get; set; }

        public virtual Model Model { get; set; }

        public int ProblemId { get; set; }

        public virtual Problem Problem { get; set; }

        public int SelectedProblemId { get; set; }

      

    }
}
