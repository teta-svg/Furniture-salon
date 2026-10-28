using FurnitureSalon.Api.Data;
using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureSalon.Api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FurnitureSalonDbContext _context;

        public OrderRepository(FurnitureSalonDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.IdClientNavigation)
                    .ThenInclude(c => c.LegalClient)
                .Include(o => o.IdClientNavigation)
                    .ThenInclude(c => c.PhysicalClient)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.IdFurnitureConfigurationNavigation)
                        .ThenInclude(fc => fc.IdModelNavigation)
                .ToListAsync();
        }
    }
}