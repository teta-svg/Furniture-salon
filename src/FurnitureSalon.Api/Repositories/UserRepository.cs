using FurnitureSalon.Api.Models;
using Microsoft.EntityFrameworkCore;
using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Data;

namespace FurnitureSalon.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FurnitureSalonDbContext _context;

        public UserRepository(FurnitureSalonDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _context.Users
                .Include(u => u.IdEmployeeNavigation)
                .FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
