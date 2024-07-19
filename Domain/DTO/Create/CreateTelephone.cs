using TelephoneShop.Models;

namespace Domain.DTO.Create
{
    public class CreateTelephone
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Catalog { get; set; }

        public virtual List<CreateCTTCost> CTTCost { get; set; }
    }
}
