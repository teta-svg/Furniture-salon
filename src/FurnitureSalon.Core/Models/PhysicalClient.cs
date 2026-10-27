using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class PhysicalClient
{
    public int IdPhysicalClient { get; set; }

    public int IdClient { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;
}
