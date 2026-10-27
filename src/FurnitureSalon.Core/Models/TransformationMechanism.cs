using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class TransformationMechanism
{
    public int IdTransformationMechanism { get; set; }

    public string Name { get; set; } = null!;

    public decimal PriceMarkup { get; set; }

    public virtual ICollection<FurnitureConfiguration> FurnitureConfigurations { get; set; } = new List<FurnitureConfiguration>();
}
