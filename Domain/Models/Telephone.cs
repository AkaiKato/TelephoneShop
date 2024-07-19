namespace TelephoneShop.Models
{
    public class Telephone
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual Catalog Catalog { get; set; } = null!;

        public virtual List<CitiesToTelephoneCost>? CitiesToTelephoneCost { get; set; }
    }
}
