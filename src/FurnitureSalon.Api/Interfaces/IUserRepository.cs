using FurnitureSalon.Api.Models;

namespace FurnitureSalon.Api.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByLoginAsync(string login);
    }
}