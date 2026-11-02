namespace FurnitureSalon.Core.DTO.Order
{
    public class OrderDictionariesResponse
    {
        public List<DictionaryItem> Models { get; set; } = [];
        public List<DictionaryItem> FabricCategories { get; set; } = [];
        public List<DictionaryItem> TransformationMechanisms { get; set; } = [];
        public List<DictionaryItem> StorageModules { get; set; } = [];
    }

    public class DictionaryItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}