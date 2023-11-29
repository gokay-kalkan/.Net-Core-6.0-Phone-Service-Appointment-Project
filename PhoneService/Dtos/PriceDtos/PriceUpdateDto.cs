namespace PhoneService.Dtos.PriceDtos
{
    public class PriceUpdateDto
    {
        public int Id { get; set; }

        public decimal PriceValue { get; set; }

        public int CityId { get; set; }

        public int DealerId { get; set; }

        public int ProblemId { get; set; }
    }
}
