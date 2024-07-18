namespace TelephoneShop.Models
{
    public class CitiesToTelephoneCost
    {
        public int Id { get; set; }
        public virtual Cities City { get; set; } = null!;

        public virtual Telephone Telephone { get; set; } = null!;

        public decimal Cost { get; set; }

    }
}
