using System.ComponentModel.DataAnnotations;

namespace PhoneService.Entities
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        public decimal PriceValue { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public int DealerId { get; set; }

        public bool Status { get; set; }
        public virtual Dealer Dealer { get; set; }

        public int ProblemId { get; set; }

        public virtual Problem Problem { get; set; }
    }
}
