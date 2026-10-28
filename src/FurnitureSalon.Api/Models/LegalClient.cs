using FurnitureSalon.Api.Models;
using System;
using System.Collections.Generic;

namespace FurnitureSalon.Api.Models;

public partial class LegalClient
{
    public int IdLegalClient { get; set; }

    public int IdClient { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public string? Kpp { get; set; }

    public string SurnameContactPerson { get; set; } = null!;

    public string NameContactPerson { get; set; } = null!;

    public string? PatronymicContactPerson { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;
}
