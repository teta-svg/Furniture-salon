using FurnitureSalon.Api.Models;
using System;
using System.Collections.Generic;

namespace FurnitureSalon.Api.Models;

public partial class ProductionOrder
{
    public int IdProductionOrder { get; set; }

    public int IdOrder { get; set; }

    public int IdEmployee { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
