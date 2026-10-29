using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Models;
using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private const string PhysicalClientType = "Физическое лицо";
        private const string LegalClientType = "Юридическое лицо";
        private const string NewItemStatus = "Ожидает производства";

        private string GetOrderStatus(ICollection<OrderItem> orderItems)
        {
            if (orderItems.Count == 0)
                return "Нет позиций";

            if (orderItems.All(i => i.CurrentStatus == "Доставлен"))
                return "Доставлен";

            if (orderItems.All(i => i.CurrentStatus == "Отменён"))
                return "Отменён";

            if (orderItems.Any(i => i.CurrentStatus == "В производстве"))
                return "В производстве";

            if (orderItems.All(i => i.CurrentStatus == "Готов"))
                return "Готов";

            if (orderItems.All(i => i.CurrentStatus == "Ожидает производства"))
                return "Ожидает производства";

            return "Выполняется";
        }

        public async Task<List<OrderListResponse>> GetAllOrdersAsync()
        {
            List<OrderListResponse> orderListResponse = [];

            List<Order> orders = await _orderRepository.GetOrdersAsync();

            foreach (var order in orders)
            {
                List<OrderItemResponse> orderItemResponse = [];

                foreach (var orderItem in order.OrderItems)
                {
                    orderItemResponse.Add(new OrderItemResponse
                    {
                        ModelName = orderItem.IdFurnitureConfigurationNavigation.IdModelNavigation.ModelName,
                        PlannedReadyDate = orderItem.PlannedReadyDate,
                        
                        Quantity = orderItem.Quantity
                    });
                }

                orderListResponse.Add(new OrderListResponse
                {
                    IdOrder = order.IdOrder,
                    ClientName = order.IdClientNavigation.ClientType == "Физическое лицо" ?
                            $"{order.IdClientNavigation.PhysicalClient.Surname} {order.IdClientNavigation.PhysicalClient.Name.Trim()[0]}." :
                            order.IdClientNavigation.LegalClient.CompanyName,
                    ClientType = order.IdClientNavigation.ClientType,
                    CreationDate = order.CreationDate,
                    TotalPrice = order.TotalPrice,
                    CurrentStatus = GetOrderStatus(order.OrderItems),
                    OrderItems = orderItemResponse
                });
            }

            return orderListResponse;
        }

        public async Task<List<OrderListResponse>> SearchOrdersAsync(OrderSearchRequest searchRequest)
        {
            List<OrderListResponse> orderListResponse = [];

            List<Order> orders = await _orderRepository.SearchOrdersAsync(searchRequest.SearchText);

            if (!string.IsNullOrWhiteSpace(searchRequest.Status))
            {
                orders = orders
                    .Where(o => GetOrderStatus(o.OrderItems) == searchRequest.Status)
                    .ToList();
            }

            foreach (var order in orders)
            {
                List<OrderItemResponse> orderItemResponse = [];

                foreach (var orderItem in order.OrderItems)
                {
                    orderItemResponse.Add(new OrderItemResponse
                    {
                        ModelName = orderItem.IdFurnitureConfigurationNavigation.IdModelNavigation.ModelName,
                        PlannedReadyDate = orderItem.PlannedReadyDate,

                        Quantity = orderItem.Quantity
                    });
                }

                orderListResponse.Add(new OrderListResponse
                {
                    IdOrder = order.IdOrder,
                    ClientName = order.IdClientNavigation.ClientType == "Физическое лицо" ?
                            $"{order.IdClientNavigation.PhysicalClient.Surname} {order.IdClientNavigation.PhysicalClient.Name.Trim()[0]}." :
                            order.IdClientNavigation.LegalClient.CompanyName,
                    ClientType = order.IdClientNavigation.ClientType,
                    CreationDate = order.CreationDate,
                    TotalPrice = order.TotalPrice,
                    CurrentStatus = GetOrderStatus(order.OrderItems),
                    OrderItems = orderItemResponse
                });
            }

            return orderListResponse;
        }
        public async Task<OrderLookupsResponse> GetLookupsAsync()
        {
            return new OrderLookupsResponse
            {
                Models = (await _orderRepository.GetModelsAsync())
                    .Select(m => new LookupItem { Id = m.IdModel, Name = m.ModelName, Price = m.BasePrice })
                    .ToList(),
                FabricCategories = (await _orderRepository.GetFabricCategoriesAsync())
                    .Select(f => new LookupItem { Id = f.IdFabricCategory, Name = f.Name, Price = f.PriceMarkup })
                    .ToList(),
                TransformationMechanisms = (await _orderRepository.GetTransformationMechanismsAsync())
                    .Select(m => new LookupItem { Id = m.IdTransformationMechanism, Name = m.Name, Price = m.PriceMarkup })
                    .ToList(),
                StorageModules = (await _orderRepository.GetStorageModulesAsync())
                    .Select(m => new LookupItem { Id = m.IdStorageModule, Name = m.ModuleName, Price = m.BasePrice })
                    .ToList()
            };
        }

        public async Task<OrderEditor?> GetOrderForEditAsync(int id)
        {
            Order? order = await _orderRepository.GetOrderByIdAsync(id);

            if (order is null)
                return null;

            var client = order.IdClientNavigation;

            var editor = new OrderEditor
            {
                IdOrder = order.IdOrder,
                IdEmployee = order.IdEmployee,
                CreationDate = order.CreationDate,
                TotalPrice = order.TotalPrice,
                CurrentStatus = GetOrderStatus(order.OrderItems),
                ClientType = client.ClientType,
                Phone = client.Phone,
                DeliveryCity = order.DeliveryCity,
                DeliveryStreet = order.DeliveryStreet,
                DeliveryBuilding = order.DeliveryBuilding,
                DeliveryApartment = order.DeliveryApartment,
                Items = order.OrderItems.Select(i =>
                {
                    var config = i.IdFurnitureConfigurationNavigation;

                    return new OrderItemEditor
                    {
                        IdOrderItem = i.IdOrderItem,
                        IdModel = config.IdModel,
                        IdFabricCategory = config.IdFabricCategory,
                        IdTransformationMechanism = config.IdTransformationMechanism,
                        Length = config.Length,
                        Width = config.Width,
                        Height = config.Height,
                        Color = config.Color,
                        StorageModuleIds = config.FurnitureConfigurationStorageModules
                            .Select(m => m.IdStorageModule)
                            .ToList(),
                        Quantity = i.Quantity,
                        PlannedReadyDate = i.PlannedReadyDate,
                        CurrentStatus = i.CurrentStatus
                    };
                }).ToList()
            };

            if (client.PhysicalClient is { } physical)
            {
                editor.Surname = physical.Surname;
                editor.Name = physical.Name;
                editor.Patronymic = physical.Patronymic;
            }

            if (client.LegalClient is { } legal)
            {
                editor.CompanyName = legal.CompanyName;
                editor.Inn = legal.Inn;
                editor.Kpp = legal.Kpp;
                editor.ContactSurname = legal.SurnameContactPerson;
                editor.ContactName = legal.NameContactPerson;
                editor.ContactPatronymic = legal.PatronymicContactPerson;
            }

            return editor;
        }

        public async Task<int> CreateOrderAsync(OrderEditor request)
        {
            Validate(request);

            var client = new Client
            {
                ClientType = request.ClientType,
                Phone = request.Phone.Trim()
            };

            ApplyClientDetails(client, request);

            var order = new Order
            {
                IdEmployee = request.IdEmployee,
                CreationDate = DateOnly.FromDateTime(DateTime.Today),
                IdClientNavigation = client
            };

            ApplyDelivery(order, request);

            foreach (var item in request.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    IdFurnitureConfigurationNavigation = await GetConfigurationAsync(item),
                    PlannedReadyDate = item.PlannedReadyDate,
                    Quantity = item.Quantity,
                    CurrentStatus = NewItemStatus
                });
            }

            order.TotalPrice = await CalculateTotalAsync(request.Items);

            return await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<bool> UpdateOrderAsync(int id, OrderEditor request)
        {
            Order? order = await _orderRepository.GetOrderByIdAsync(id);

            if (order is null)
                return false;

            request.ClientType = order.IdClientNavigation.ClientType;

            Validate(request);

            order.IdClientNavigation.Phone = request.Phone.Trim();
            ApplyClientDetails(order.IdClientNavigation, request);
            ApplyDelivery(order, request);

            var keptIds = request.Items
                .Where(i => i.IdOrderItem != 0)
                .Select(i => i.IdOrderItem)
                .ToHashSet();

            var removedItems = order.OrderItems
                .Where(i => !keptIds.Contains(i.IdOrderItem))
                .ToList();

            foreach (var item in request.Items)
            {
                var configuration = await GetConfigurationAsync(item);

                if (item.IdOrderItem == 0)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        IdFurnitureConfigurationNavigation = configuration,
                        PlannedReadyDate = item.PlannedReadyDate,
                        Quantity = item.Quantity,
                        CurrentStatus = NewItemStatus
                    });

                    continue;
                }

                var existing = order.OrderItems.FirstOrDefault(i => i.IdOrderItem == item.IdOrderItem)
                    ?? throw new ArgumentException($"Позиция {item.IdOrderItem} не принадлежит заказу {id}.");

                existing.IdFurnitureConfigurationNavigation = configuration;
                existing.PlannedReadyDate = item.PlannedReadyDate;
                existing.Quantity = item.Quantity;
            }

            order.TotalPrice = await CalculateTotalAsync(request.Items);

            await _orderRepository.UpdateOrderAsync(order, removedItems);

            return true;
        }

        private Task<FurnitureConfiguration> GetConfigurationAsync(OrderItemEditor item)
        {
            return _orderRepository.GetOrCreateConfigurationAsync(
                item.IdModel, item.IdFabricCategory, item.IdTransformationMechanism,
                item.Length, item.Width, item.Height, item.Color.Trim(),
                item.StorageModuleIds);
        }

        private static void ApplyClientDetails(Client client, OrderEditor request)
        {
            if (client.ClientType == PhysicalClientType)
            {
                var physical = client.PhysicalClient ??= new PhysicalClient();

                physical.Surname = request.Surname!.Trim();
                physical.Name = request.Name!.Trim();
                physical.Patronymic = NullIfEmpty(request.Patronymic);
            }
            else
            {
                var legal = client.LegalClient ??= new LegalClient();

                legal.CompanyName = request.CompanyName!.Trim();
                legal.Inn = request.Inn!.Trim();
                legal.Kpp = NullIfEmpty(request.Kpp);
                legal.SurnameContactPerson = request.ContactSurname!.Trim();
                legal.NameContactPerson = request.ContactName!.Trim();
                legal.PatronymicContactPerson = NullIfEmpty(request.ContactPatronymic);
            }
        }

        private static void ApplyDelivery(Order order, OrderEditor request)
        {
            order.DeliveryCity = request.DeliveryCity.Trim();
            order.DeliveryStreet = request.DeliveryStreet.Trim();
            order.DeliveryBuilding = request.DeliveryBuilding.Trim();
            order.DeliveryApartment = NullIfEmpty(request.DeliveryApartment);
        }

        private static string? NullIfEmpty(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private async Task<decimal> CalculateTotalAsync(IEnumerable<OrderItemEditor> items)
        {
            var models = (await _orderRepository.GetModelsAsync()).ToDictionary(m => m.IdModel, m => m.BasePrice);
            var fabrics = (await _orderRepository.GetFabricCategoriesAsync()).ToDictionary(f => f.IdFabricCategory, f => f.PriceMarkup);
            var mechanisms = (await _orderRepository.GetTransformationMechanismsAsync()).ToDictionary(m => m.IdTransformationMechanism, m => m.PriceMarkup);
            var modules = (await _orderRepository.GetStorageModulesAsync()).ToDictionary(m => m.IdStorageModule, m => m.BasePrice);

            decimal total = 0;

            foreach (var item in items)
            {
                if (!models.TryGetValue(item.IdModel, out var modelPrice))
                    throw new ArgumentException($"Модель {item.IdModel} не найдена.");

                if (!fabrics.TryGetValue(item.IdFabricCategory, out var fabricMarkup))
                    throw new ArgumentException($"Категория ткани {item.IdFabricCategory} не найдена.");

                if (!mechanisms.TryGetValue(item.IdTransformationMechanism, out var mechanismMarkup))
                    throw new ArgumentException($"Механизм трансформации {item.IdTransformationMechanism} не найден.");

                decimal modulesPrice = 0;

                foreach (var moduleId in item.StorageModuleIds.Distinct())
                {
                    if (!modules.TryGetValue(moduleId, out var modulePrice))
                        throw new ArgumentException($"Модуль хранения {moduleId} не найден.");

                    modulesPrice += modulePrice;
                }

                total += (modelPrice + fabricMarkup + mechanismMarkup + modulesPrice) * item.Quantity;
            }

            return total;
        }

        private static void Validate(OrderEditor request)
        {
            if (request.ClientType != PhysicalClientType && request.ClientType != LegalClientType)
                throw new ArgumentException("Неизвестный тип клиента.");

            if (string.IsNullOrWhiteSpace(request.Phone))
                throw new ArgumentException("Не указан телефон клиента.");

            if (request.ClientType == PhysicalClientType)
            {
                if (string.IsNullOrWhiteSpace(request.Surname) || string.IsNullOrWhiteSpace(request.Name))
                    throw new ArgumentException("Не указаны фамилия и имя клиента.");
            }
            else if (string.IsNullOrWhiteSpace(request.CompanyName)
                  || string.IsNullOrWhiteSpace(request.Inn)
                  || string.IsNullOrWhiteSpace(request.ContactSurname)
                  || string.IsNullOrWhiteSpace(request.ContactName))
            {
                throw new ArgumentException("Не заполнены данные юридического лица.");
            }

            if (string.IsNullOrWhiteSpace(request.DeliveryCity)
             || string.IsNullOrWhiteSpace(request.DeliveryStreet)
             || string.IsNullOrWhiteSpace(request.DeliveryBuilding))
            {
                throw new ArgumentException("Не заполнен адрес доставки.");
            }

            if (request.Items.Count == 0)
                throw new ArgumentException("В заказе нет позиций.");

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new ArgumentException("Количество должно быть больше нуля.");

                if (item.Length <= 0 || item.Width <= 0 || item.Height <= 0)
                    throw new ArgumentException("Размеры должны быть больше нуля.");

                if (string.IsNullOrWhiteSpace(item.Color))
                    throw new ArgumentException("Не указан цвет.");
            }
        }
    }
}
