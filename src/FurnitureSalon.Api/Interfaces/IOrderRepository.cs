using FurnitureSalon.Api.Models;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> SearchOrdersAsync(string? searchText);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<int> CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order, IReadOnlyCollection<OrderItem> removedItems);

        Task<List<Model>> GetModelsAsync();
        Task<List<FabricCategory>> GetFabricCategoriesAsync();
        Task<List<TransformationMechanism>> GetTransformationMechanismsAsync();
        Task<List<StorageModule>> GetStorageModulesAsync();

        Task<FurnitureConfiguration> GetOrCreateConfigurationAsync(
            int idModel, int idFabricCategory, int idTransformationMechanism,
            int length, int width, int height, string color,
            IReadOnlyCollection<int> storageModuleIds);
    }
}