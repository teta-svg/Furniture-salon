using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class Model
{
    public int IdModel { get; set; }

    public string ModelName { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public virtual ICollection<FurnitureConfiguration> FurnitureConfigurations { get; set; } = new List<FurnitureConfiguration>();
}
