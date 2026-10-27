using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class MaterialSupplier
{
    public int IdMaterialSupplier { get; set; }

    public int IdSupplier { get; set; }

    public int IdMaterial { get; set; }

    public int DeliveryTime { get; set; }

    public decimal Price { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    public virtual Supplier IdSupplierNavigation { get; set; } = null!;
}
