using FurnitureSalon.Api.Models;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> SearchOrdersAsync(string? searchText);
        Task<Order?> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order, IReadOnlyCollection<OrderItem> removedItems);

        Task<List<Model>> GetModelsAsync();
        Task<List<FabricCategory>> GetFabricCategoriesAsync();
        Task<List<TransformationMechanism>> GetTransformationMechanismsAsync();
        Task<List<StorageModule>> GetStorageModulesAsync();
    }
}