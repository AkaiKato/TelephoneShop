namespace Domain.DTO.Add
{
    public class AddCityToTelephone
    {
        public int TelephoneId { get; set; }

        public List<NewCityToTelephone> Cities { get; set; } = null!;
    }
}
