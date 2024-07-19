namespace Domain.DTO.Update
{
    public class UpdateTelephone
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int CatalogId { get; set; }

        public virtual List<UpdateCityCost>? CityCost { get; set; }
    }
}
