using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class DeliveryNote
{
    public int IdDeliveryNote { get; set; }

    public int IdOrder { get; set; }

    public int IdEmployee { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateOnly? LoadingDate { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
