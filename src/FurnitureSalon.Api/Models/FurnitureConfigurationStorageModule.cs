using System;
using System.Collections.Generic;

namespace FurnitureSalon.Api.Models;

public partial class FurnitureConfigurationStorageModule
{
    public int IdFurnitureConfigurationStorageModule { get; set; }

    public int IdFurnitureConfiguration { get; set; }

    public int IdStorageModule { get; set; }

    public virtual FurnitureConfiguration IdFurnitureConfigurationNavigation { get; set; } = null!;

    public virtual StorageModule IdStorageModuleNavigation { get; set; } = null!;
}
