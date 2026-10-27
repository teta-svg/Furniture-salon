using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class FurnitureConfiguration
{
    public int IdFurnitureConfiguration { get; set; }

    public int IdModel { get; set; }

    public int IdFabricCategory { get; set; }

    public int IdTransformationMechanism { get; set; }

    public int Length { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public string Color { get; set; } = null!;

    public virtual ICollection<FurnitureConfigurationMaterial> FurnitureConfigurationMaterials { get; set; } = new List<FurnitureConfigurationMaterial>();

    public virtual ICollection<FurnitureConfigurationStorageModule> FurnitureConfigurationStorageModules { get; set; } = new List<FurnitureConfigurationStorageModule>();

    public virtual FabricCategory IdFabricCategoryNavigation { get; set; } = null!;

    public virtual Model IdModelNavigation { get; set; } = null!;

    public virtual TransformationMechanism IdTransformationMechanismNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
