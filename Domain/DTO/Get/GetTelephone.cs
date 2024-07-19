namespace Domain.DTO.Get
{
    public class GetTelephone
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int CatalogId { get; set; }

        public string? CatalogName { get; set; }

        public virtual List<GetCityCost>? CityCost { get; set; }
    }
}
