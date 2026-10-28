namespace FurnitureSalon.Api.Models;

public partial class OrderItem
{
    public int IdOrderItem { get; set; }

    public int IdOrder { get; set; }

    public int IdFurnitureConfiguration { get; set; }

    public DateOnly PlannedReadyDate { get; set; }

    public string CurrentStatus { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual FurnitureConfiguration IdFurnitureConfigurationNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
