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


        public async Task<List<Order>> SearchOrdersAsync(string? searchText)
        {
            var query = _context.Orders
                .Include(o => o.IdClientNavigation)
                    .ThenInclude(c => c.LegalClient)
                .Include(o => o.IdClientNavigation)
                    .ThenInclude(c => c.PhysicalClient)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.IdFurnitureConfigurationNavigation)
                        .ThenInclude(fc => fc.IdModelNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string search = searchText.Trim();

                if (int.TryParse(search, out int orderId))
                {
                    query = query.Where(o =>
                        o.IdOrder == orderId ||
                        (o.IdClientNavigation.PhysicalClient != null &&
                         (o.IdClientNavigation.PhysicalClient.Surname.Contains(search) ||
                          o.IdClientNavigation.PhysicalClient.Name.Contains(search))) ||
                        (o.IdClientNavigation.LegalClient != null &&
                         o.IdClientNavigation.LegalClient.CompanyName.Contains(search)));
                }
                else
                {
                    query = query.Where(o =>
                        (o.IdClientNavigation.PhysicalClient != null &&
                         (o.IdClientNavigation.PhysicalClient.Surname.Contains(search) ||
                          o.IdClientNavigation.PhysicalClient.Name.Contains(search))) ||
                        (o.IdClientNavigation.LegalClient != null &&
                         o.IdClientNavigation.LegalClient.CompanyName.Contains(search)));
                }
            }

            return await query.ToListAsync();
        }
    }
}