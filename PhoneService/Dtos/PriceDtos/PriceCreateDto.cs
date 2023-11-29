namespace PhoneService.Dtos.PriceDtos
{
    public class PriceCreateDto
    {

        public decimal PriceValue { get; set; }

        public int CityId { get; set; }

        public int DealerId { get; set; }
        public int ProblemId { get; set; }

        public decimal SelectedProblemPrice { get; set; }
    }
}
