namespace FurnitureSalon.Core.DTO.Order
{
    public class OrderListResponse
    {
        public int IdOrder { get; set; }
        public string ClientName { get; set; } = null!;
        public string ClientType { get; set; } = null!;
        public DateOnly CreationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = [];
    }

    public class OrderItemResponse
    {
        public string ModelName { get; set; } = null!;
        public DateOnly PlannedReadyDate { get; set; }
        public string CurrentStatus { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
