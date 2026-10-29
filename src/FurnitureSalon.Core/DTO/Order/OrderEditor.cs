namespace FurnitureSalon.Core.DTO.Order
{
    public class OrderEditor
    {
        public int IdOrder { get; set; }
        public int IdEmployee { get; set; }
        public DateOnly CreationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;
        public string ClientType { get; set; } = "Физическое лицо";
        public string Phone { get; set; } = null!;
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public string? CompanyName { get; set; }
        public string? Inn { get; set; }
        public string? Kpp { get; set; }
        public string? ContactSurname { get; set; }
        public string? ContactName { get; set; }
        public string? ContactPatronymic { get; set; }
        public string DeliveryCity { get; set; } = null!;
        public string DeliveryStreet { get; set; } = null!;
        public string DeliveryBuilding { get; set; } = null!;
        public string? DeliveryApartment { get; set; }

        public List<OrderItemEditor> Items { get; set; } = [];
    }

    public class OrderItemEditor
    {
        public int IdOrderItem { get; set; }

        public int IdModel { get; set; }
        public int IdFabricCategory { get; set; }
        public int IdTransformationMechanism { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; } = null!;
        public List<int> StorageModuleIds { get; set; } = [];
        public int Quantity { get; set; }
        public DateOnly PlannedReadyDate { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;
    }
}
