using Microsoft.EntityFrameworkCore;

namespace TelephoneShop.Models
{
    public class Catalog
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual Catalog? ParentCatalog { get; set; }

    }
}
