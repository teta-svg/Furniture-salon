using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string EmployeeRole { get; set; } = null!;

    public virtual ICollection<DeliveryNote> DeliveryNotes { get; set; } = new List<DeliveryNote>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; } = new List<ProductionOrder>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
