using FurnitureSalon.Api.Models;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync();
    }
}