using TelephoneShop.Models;

namespace Domain.DTO.Get
{
    public class GetCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual Catalog? ParentCatalog { get; set; }
    }
}
