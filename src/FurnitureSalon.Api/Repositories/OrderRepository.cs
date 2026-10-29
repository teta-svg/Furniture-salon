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

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.IdClientNavigation)
                    .ThenInclude(c => c.PhysicalClient)
                .Include(o => o.IdClientNavigation)
                    .ThenInclude(c => c.LegalClient)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.IdFurnitureConfigurationNavigation)
                        .ThenInclude(c => c.IdModelNavigation)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.IdFurnitureConfigurationNavigation)
                        .ThenInclude(c => c.IdFabricCategoryNavigation)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.IdFurnitureConfigurationNavigation)
                        .ThenInclude(c => c.IdTransformationMechanismNavigation)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.IdFurnitureConfigurationNavigation)
                        .ThenInclude(c => c.FurnitureConfigurationStorageModules)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.IdOrder;
        }

        public async Task UpdateOrderAsync(Order order, IReadOnlyCollection<OrderItem> removedItems)
        {
            _context.OrderItems.RemoveRange(removedItems);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Model>> GetModelsAsync()
        {
            return await _context.Models.AsNoTracking().OrderBy(m => m.ModelName).ToListAsync();
        }

        public async Task<List<FabricCategory>> GetFabricCategoriesAsync()
        {
            return await _context.FabricCategories.AsNoTracking().OrderBy(f => f.Name).ToListAsync();
        }

        public async Task<List<TransformationMechanism>> GetTransformationMechanismsAsync()
        {
            return await _context.TransformationMechanisms.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<List<StorageModule>> GetStorageModulesAsync()
        {
            return await _context.StorageModules.AsNoTracking().OrderBy(m => m.ModuleName).ToListAsync();
        }

        public async Task<FurnitureConfiguration> GetOrCreateConfigurationAsync(
            int idModel, int idFabricCategory, int idTransformationMechanism,
            int length, int width, int height, string color,
            IReadOnlyCollection<int> storageModuleIds)
        {
            var moduleIds = storageModuleIds.Distinct().OrderBy(id => id).ToList();

            var candidates = await _context.FurnitureConfigurations
                .Include(c => c.FurnitureConfigurationStorageModules)
                .Where(c => c.IdModel == idModel
                         && c.IdFabricCategory == idFabricCategory
                         && c.IdTransformationMechanism == idTransformationMechanism
                         && c.Length == length
                         && c.Width == width
                         && c.Height == height
                         && c.Color == color)
                .ToListAsync();

            var existing = candidates.FirstOrDefault(c =>
                c.FurnitureConfigurationStorageModules
                    .Select(m => m.IdStorageModule)
                    .OrderBy(id => id)
                    .SequenceEqual(moduleIds));

            if (existing != null)
                return existing;

            return new FurnitureConfiguration
            {
                IdModel = idModel,
                IdFabricCategory = idFabricCategory,
                IdTransformationMechanism = idTransformationMechanism,
                Length = length,
                Width = width,
                Height = height,
                Color = color,
                FurnitureConfigurationStorageModules = moduleIds
                    .Select(id => new FurnitureConfigurationStorageModule { IdStorageModule = id })
                    .ToList()
            };
        }
    }
}