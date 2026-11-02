using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderListResponse>> GetAllOrdersAsync();
        Task<List<OrderListResponse>> SearchOrdersAsync(OrderSearchRequest searchRequest);
        Task<OrderResponse?> GetOrderByIdAsync(int id);
        Task<OrderDictionariesResponse> GetDictAsync();
        Task CreateOrderAsync(CreateOrderRequest request);
        Task<bool> UpdateOrderAsync(int id, EditOrderRequest request);
    }
}