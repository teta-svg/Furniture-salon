using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class FurnitureConfigurationMaterial
{
    public int IdFurnitureConfigurationMaterial { get; set; }

    public int IdFurnitureConfiguration { get; set; }

    public int IdMaterial { get; set; }

    public decimal RequiredQuantity { get; set; }

    public virtual FurnitureConfiguration IdFurnitureConfigurationNavigation { get; set; } = null!;

    public virtual Material IdMaterialNavigation { get; set; } = null!;
}
