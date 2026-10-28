using FurnitureSalon.Api.Models;

namespace FurnitureSalon.Api.Models;

public partial class User
{
    public int IdUser { get; set; }

    public int IdEmployee { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;
}
