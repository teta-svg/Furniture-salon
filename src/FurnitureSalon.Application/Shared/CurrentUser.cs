using FurnitureSalon.Core.DTO.Login;

namespace FurnitureSalon.Application.Shared
{
    public class CurrentUser : ICurrentUser
    {
        public LoginResponse? User { get; set; }
    }
}