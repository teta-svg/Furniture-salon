using System;
using System.Collections.Generic;

namespace FurnitureSalon.Api.Models;

public partial class Supplier
{
    public int IdSupplier { get; set; }

    public string SupplierName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<MaterialSupplier> MaterialSuppliers { get; set; } = new List<MaterialSupplier>();
}
