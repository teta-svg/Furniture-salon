using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class Material
{
    public int IdMaterial { get; set; }

    public string MaterialName { get; set; } = null!;

    public string MaterialType { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal QuantityInStock { get; set; }

    public virtual ICollection<FurnitureConfigurationMaterial> FurnitureConfigurationMaterials { get; set; } = new List<FurnitureConfigurationMaterial>();

    public virtual ICollection<MaterialSupplier> MaterialSuppliers { get; set; } = new List<MaterialSupplier>();
}
