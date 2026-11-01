namespace FurnitureSalon.Core.DTO.Order
{
    public class CreateOrderRequest
    {
       public OrderDto Order {  get; set; } = new();
    }

    public class EditOrderRequest
    {
        public int OrderId { get; set; }
        public OrderDto Order { get; set; } = new();
    }

    public class OrderResponse
    {
        public int OrderId { get; set; }
        public OrderDto Order { get; set; } = new();
    }

    public class OrderDto
    {
        public ClientDto Client { get; set; } = new();
        public int IdEmployee { get; set; }
        public List<OrderItemConfigurationDto> OrderItem { get; set; } = new();
        public DateOnly CreationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryCity { get; set; } = null!;
        public string DeliveryStreet { get; set; } = null!;
        public string DeliveryBuilding { get; set; } = null!;
        public string? DeliveryApartment { get; set; }
    }

    public class ClientDto
    {
        public string ClientType { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public LegalClientDto? LegalClient { get; set; }
        public PhysicalClientDto? PhysicalClient { get; set; }
    }

    public class LegalClientDto
    {
        public string CompanyName { get; set; } = null!;
        public string Inn { get; set; } = null!;
        public string? Kpp { get; set; }
        public string SurnameContactPerson { get; set; } = null!;
        public string NameContactPerson { get; set; } = null!;
        public string? PatronymicContactPerson { get; set; }
    }

    public class PhysicalClientDto
    {
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
    }

    public class OrderItemConfigurationDto
    {
        public ConfigurationDto Configuration { get; set; } = new();
        public DateTime? PlannedReadyDate { get; set; }
        public int Quantity { get; set; }
        public string CurrentStatus { get; set; } = null!;
    }

    public class ConfigurationDto
    {
        public int IdModel { get; set; }
        public int IdFabricCategory { get; set; }
        public int IdTransformationMechanism { get; set; }
        public List<int> StorageModules { get; set; } = [];
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; } = null!;
    }
}
