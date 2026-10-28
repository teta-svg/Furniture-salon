using FurnitureSalon.Core.DTO.Login;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
