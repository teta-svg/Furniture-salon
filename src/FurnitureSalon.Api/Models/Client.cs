namespace FurnitureSalon.Api.Models;

public partial class Client
{
    public int IdClient { get; set; }

    public string ClientType { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual LegalClient? LegalClient { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual PhysicalClient? PhysicalClient { get; set; }
}
