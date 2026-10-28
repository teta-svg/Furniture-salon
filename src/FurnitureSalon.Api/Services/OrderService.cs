using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Models;
using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private string GetOrderStatus(ICollection<OrderItem> orderItems)
        {
            if (orderItems.Count == 0)
                return "Нет позиций";

            if (orderItems.All(i => i.CurrentStatus == "Доставлен"))
                return "Доставлен";

            if (orderItems.All(i => i.CurrentStatus == "Отменён"))
                return "Отменён";

            if (orderItems.Any(i => i.CurrentStatus == "В производстве"))
                return "В производстве";

            if (orderItems.All(i => i.CurrentStatus == "Готов"))
                return "Готов";

            if (orderItems.All(i => i.CurrentStatus == "Ожидает производства"))
                return "Ожидает производства";

            return "Выполняется";
        }

        public async Task<List<OrderListResponse>> GetAllOrdersAsync()
        {
            List<OrderListResponse> orderListResponse = [];

            List<Order> orders = await _orderRepository.GetOrdersAsync();

            foreach (var order in orders)
            {
                List<OrderItemResponse> orderItemResponse = [];

                foreach (var orderItem in order.OrderItems)
                {
                    orderItemResponse.Add(new OrderItemResponse
                    {
                        ModelName = orderItem.IdFurnitureConfigurationNavigation.IdModelNavigation.ModelName,
                        PlannedReadyDate = orderItem.PlannedReadyDate,
                        
                        Quantity = orderItem.Quantity
                    });
                }

                orderListResponse.Add(new OrderListResponse
                {
                    IdOrder = order.IdOrder,
                    ClientName = order.IdClientNavigation.ClientType == "Физическое лицо" ?
                            $"{order.IdClientNavigation.PhysicalClient.Surname} {order.IdClientNavigation.PhysicalClient.Name.Trim()[0]}." :
                            order.IdClientNavigation.LegalClient.CompanyName,
                    ClientType = order.IdClientNavigation.ClientType,
                    CreationDate = order.CreationDate,
                    TotalPrice = order.TotalPrice,
                    CurrentStatus = GetOrderStatus(order.OrderItems),
                    OrderItems = orderItemResponse
                });
            }

            return orderListResponse;
        }

        public async Task<List<OrderListResponse>> SearchOrdersAsync(OrderSearchRequest searchRequest)
        {
            List<OrderListResponse> orderListResponse = [];

            List<Order> orders = await _orderRepository.SearchOrdersAsync(searchRequest.SearchText);

            if (!string.IsNullOrWhiteSpace(searchRequest.Status))
            {
                orders = orders
                    .Where(o => GetOrderStatus(o.OrderItems) == searchRequest.Status)
                    .ToList();
            }

            foreach (var order in orders)
            {
                List<OrderItemResponse> orderItemResponse = [];

                foreach (var orderItem in order.OrderItems)
                {
                    orderItemResponse.Add(new OrderItemResponse
                    {
                        ModelName = orderItem.IdFurnitureConfigurationNavigation.IdModelNavigation.ModelName,
                        PlannedReadyDate = orderItem.PlannedReadyDate,

                        Quantity = orderItem.Quantity
                    });
                }

                orderListResponse.Add(new OrderListResponse
                {
                    IdOrder = order.IdOrder,
                    ClientName = order.IdClientNavigation.ClientType == "Физическое лицо" ?
                            $"{order.IdClientNavigation.PhysicalClient.Surname} {order.IdClientNavigation.PhysicalClient.Name.Trim()[0]}." :
                            order.IdClientNavigation.LegalClient.CompanyName,
                    ClientType = order.IdClientNavigation.ClientType,
                    CreationDate = order.CreationDate,
                    TotalPrice = order.TotalPrice,
                    CurrentStatus = GetOrderStatus(order.OrderItems),
                    OrderItems = orderItemResponse
                });
            }

            return orderListResponse;
        }
    }
}
