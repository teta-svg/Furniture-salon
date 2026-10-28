using System;
using System.Collections.Generic;

namespace FurnitureSalon.Api.Models;

public partial class StorageModule
{
    public int IdStorageModule { get; set; }

    public string ModuleName { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public virtual ICollection<FurnitureConfigurationStorageModule> FurnitureConfigurationStorageModules { get; set; } = new List<FurnitureConfigurationStorageModule>();
}
