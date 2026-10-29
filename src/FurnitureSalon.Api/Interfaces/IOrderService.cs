using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderListResponse>> GetAllOrdersAsync();
        Task<List<OrderListResponse>> SearchOrdersAsync(OrderSearchRequest searchRequest);
        Task<OrderLookupsResponse> GetLookupsAsync();
        Task<OrderEditor?> GetOrderForEditAsync(int id);
        Task<int> CreateOrderAsync(OrderEditor request);
        Task<bool> UpdateOrderAsync(int id, OrderEditor request);
    }
}