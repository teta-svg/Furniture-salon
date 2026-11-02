using FurnitureSalon.Core.DTO.Login;

namespace FurnitureSalon.Application.Shared
{
    public interface ICurrentUser
    {
        LoginResponse? User { get; set; }
    }
}