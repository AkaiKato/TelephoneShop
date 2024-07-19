namespace Domain.DTO.Update
{
    public class UpdateCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? ParentCatalog { get; set; }
    }
}
