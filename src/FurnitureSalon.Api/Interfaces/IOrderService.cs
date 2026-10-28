using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderListResponse>> GetAllOrdersAsync();
        Task<List<OrderListResponse>> SearchOrdersAsync(OrderSearchRequest searchRequest);
    }
}
