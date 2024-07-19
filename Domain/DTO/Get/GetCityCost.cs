namespace Domain.DTO.Get
{
    public class GetCityCost
    {
        public int CityId { get; set; }

        public string City { get; set; } = null!;

        public decimal Cost { get; set; }
    }
}
