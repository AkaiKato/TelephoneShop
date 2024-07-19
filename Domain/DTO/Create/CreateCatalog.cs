using TelephoneShop.Models;

namespace Domain.DTO.Create
{
    public class CreateCatalog
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? ParentCatalog { get; set; }
    }
}
