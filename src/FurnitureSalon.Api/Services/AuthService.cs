using FurnitureSalon.Core.DTO.Login;
using FurnitureSalon.Api.Models;
using FurnitureSalon.Api.Interfaces;

namespace FurnitureSalon.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            LoginResponse? response = new() { IsSuccess = false };

            User? user = await _userRepository.GetByLoginAsync(request.Login);

            if (user != null && user.Password == request.Password)
            {
                response.Surname = user.IdEmployeeNavigation.Surname;
                response.Name = user.IdEmployeeNavigation.Name;
                response.Patronymic = user.IdEmployeeNavigation.Patronymic;
                response.EmployeeRole = user.IdEmployeeNavigation.EmployeeRole;
                response.Role = user.Role;
                response.IsSuccess = true;
            }

            return response;
        }
    }
}