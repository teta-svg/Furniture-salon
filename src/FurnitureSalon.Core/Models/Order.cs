using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public int IdClient { get; set; }

    public int IdEmployee { get; set; }

    public DateOnly CreationDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string DeliveryCity { get; set; } = null!;

    public string DeliveryStreet { get; set; } = null!;

    public string DeliveryBuilding { get; set; } = null!;

    public string? DeliveryApartment { get; set; }

    public virtual DeliveryNote? DeliveryNote { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ProductionOrder? ProductionOrder { get; set; }
}
