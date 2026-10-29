namespace FurnitureSalon.Core.DTO.Order
{
    public class OrderLookupsResponse
    {
        public List<LookupItem> Models { get; set; } = [];
        public List<LookupItem> FabricCategories { get; set; } = [];
        public List<LookupItem> TransformationMechanisms { get; set; } = [];
        public List<LookupItem> StorageModules { get; set; } = [];
    }

    public class LookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
