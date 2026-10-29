using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Application.ViewModels
{
    public class OrderItemRow
    {
        public OrderItemRow(OrderItemEditor data, string displayName, decimal totalPrice)
        {
            Data = data;
            DisplayName = displayName;
            TotalPrice = totalPrice;
        }

        public OrderItemEditor Data { get; }
        public string DisplayName { get; }
        public decimal TotalPrice { get; }

        public int Quantity => Data.Quantity;
        public DateOnly PlannedReadyDate => Data.PlannedReadyDate;
        public string CurrentStatus => Data.CurrentStatus;
    }
}
