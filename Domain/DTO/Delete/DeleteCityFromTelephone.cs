namespace Domain.DTO.Delete
{
    public class DeleteCityFromTelephone
    {
        public int TelephoneId { get; set; }

        public List<int> CityIds { get; set; } = null!;
    }
}
