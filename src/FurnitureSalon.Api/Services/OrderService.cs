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
                            $"{order.IdClientNavigation.PhysicalClient!.Surname} {order.IdClientNavigation.PhysicalClient.Name.Trim()[0]}." :
                            order.IdClientNavigation.LegalClient!.CompanyName,
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
                            $"{order.IdClientNavigation.PhysicalClient!.Surname} {order.IdClientNavigation.PhysicalClient.Name.Trim()[0]}." :
                            order.IdClientNavigation.LegalClient!.CompanyName,
                    ClientType = order.IdClientNavigation.ClientType,
                    CreationDate = order.CreationDate,
                    TotalPrice = order.TotalPrice,
                    CurrentStatus = GetOrderStatus(order.OrderItems),
                    OrderItems = orderItemResponse
                });
            }

            return orderListResponse;
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order is null)
                return null;

            return new OrderResponse
            {
                OrderId = order.IdOrder,
                Order = MapOrderToDto(order)
            };
        }

        private static OrderDto MapOrderToDto(Order order)
        {
            return new OrderDto
            {
                Client = MapClientToDto(order.IdClientNavigation),
                IdEmployee = order.IdEmployee,
                OrderItem = order.OrderItems.Select(MapOrderItemToDto).ToList(),
                TotalPrice = order.TotalPrice,
                CreationDate = order.CreationDate,
                DeliveryCity = order.DeliveryCity,
                DeliveryStreet = order.DeliveryStreet,
                DeliveryBuilding = order.DeliveryBuilding,
                DeliveryApartment = order.DeliveryApartment
            };
        }

        private static ClientDto MapClientToDto(Client client)
        {
            return new ClientDto
            {
                ClientType = client.ClientType,
                Phone = client.Phone,

                PhysicalClient = client.ClientType == PhysicalClientType
                    ? new PhysicalClientDto
                    {
                        Surname = client.PhysicalClient!.Surname,
                        Name = client.PhysicalClient.Name,
                        Patronymic = client.PhysicalClient.Patronymic
                    }
                    : null,

                LegalClient = client.ClientType == LegalClientType
                    ? new LegalClientDto
                    {
                        CompanyName = client.LegalClient!.CompanyName,
                        Inn = client.LegalClient.Inn,
                        Kpp = client.LegalClient.Kpp,
                        SurnameContactPerson = client.LegalClient.SurnameContactPerson,
                        NameContactPerson = client.LegalClient.NameContactPerson,
                        PatronymicContactPerson = client.LegalClient.PatronymicContactPerson
                    }
                    : null
            };
        }

        private static OrderItemConfigurationDto MapOrderItemToDto(OrderItem item)
        {
            var config = item.IdFurnitureConfigurationNavigation;

            return new OrderItemConfigurationDto
            {
                IdOrderItem = item.IdOrderItem,
                PlannedReadyDate = item.PlannedReadyDate,
                Quantity = item.Quantity,
                CurrentStatus = item.CurrentStatus,

                Configuration = new ConfigurationDto
                {
                    IdModel = config.IdModel,
                    IdFabricCategory = config.IdFabricCategory,
                    IdTransformationMechanism = config.IdTransformationMechanism,
                    Length = config.Length,
                    Width = config.Width,
                    Height = config.Height,
                    Color = config.Color,

                    StorageModules = config.FurnitureConfigurationStorageModules
                        .Select(m => m.IdStorageModule)
                        .ToList()
                }
            };
        }

        public async Task<OrderDictionariesResponse> GetDictAsync()
        {
            return new OrderDictionariesResponse
            {
                Models = (await _orderRepository.GetModelsAsync())
                    .Select(m => new DictionaryItem { Id = m.IdModel, Name = m.ModelName, Price = m.BasePrice })
                    .ToList(),
                FabricCategories = (await _orderRepository.GetFabricCategoriesAsync())
                    .Select(f => new DictionaryItem { Id = f.IdFabricCategory, Name = f.Name, Price = f.PriceMarkup })
                    .ToList(),
                TransformationMechanisms = (await _orderRepository.GetTransformationMechanismsAsync())
                    .Select(m => new DictionaryItem { Id = m.IdTransformationMechanism, Name = m.Name, Price = m.PriceMarkup })
                    .ToList(),
                StorageModules = (await _orderRepository.GetStorageModulesAsync())
                    .Select(m => new DictionaryItem { Id = m.IdStorageModule, Name = m.ModuleName, Price = m.BasePrice })
                    .ToList()
            };
        }

        public async Task CreateOrderAsync(CreateOrderRequest request)
        {
            Validate(request.Order);

            var dto = request.Order;

            var order = new Order
            {
                IdEmployee = dto.IdEmployee,
                CreationDate = DateOnly.FromDateTime(DateTime.Today),
                IdClientNavigation = CreateClient(dto.Client)
            };

            ApplyDelivery(order, dto);

            foreach (var item in dto.OrderItem)
            {
                order.OrderItems.Add(new OrderItem
                {
                    IdFurnitureConfigurationNavigation =
                        CreateConfiguration(item.Configuration),

                    PlannedReadyDate = item.PlannedReadyDate,
                    Quantity = item.Quantity,
                    CurrentStatus = NewItemStatus
                });
            }

            order.TotalPrice = await CalculateTotalAsync(dto.OrderItem);

            await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<bool> UpdateOrderAsync(int id, EditOrderRequest request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order is null)
                return false;

            var dto = request.Order;

            dto.Client.ClientType = order.IdClientNavigation.ClientType;

            Validate(dto);

            order.IdClientNavigation.Phone = dto.Client.Phone.Trim();
            ApplyClientDetails(order.IdClientNavigation, dto.Client);
            ApplyDelivery(order, dto);

            var incomingIds = dto.OrderItem
                .Where(i => i.IdOrderItem != 0)
                .Select(i => i.IdOrderItem)
                .ToHashSet();

            var removedItems = order.OrderItems
                .Where(i => !incomingIds.Contains(i.IdOrderItem))
                .ToList();

            foreach (var itemDto in dto.OrderItem)
            {
                if (itemDto.IdOrderItem == 0)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        IdFurnitureConfigurationNavigation =
                            CreateConfiguration(itemDto.Configuration),

                        PlannedReadyDate = itemDto.PlannedReadyDate,
                        Quantity = itemDto.Quantity,
                        CurrentStatus = NewItemStatus
                    });

                    continue;
                }

                var existing = order.OrderItems.FirstOrDefault(
                    i => i.IdOrderItem == itemDto.IdOrderItem);

                if (existing is null || removedItems.Contains(existing))
                {
                    throw new ArgumentException(
                        $"Позиция {itemDto.IdOrderItem} не принадлежит заказу {id}.");
                }

                existing.IdFurnitureConfigurationNavigation =
                    CreateConfiguration(itemDto.Configuration);

                existing.PlannedReadyDate = itemDto.PlannedReadyDate;
                existing.Quantity = itemDto.Quantity;
            }

            order.TotalPrice = await CalculateTotalAsync(dto.OrderItem);

            await _orderRepository.UpdateOrderAsync(order, removedItems);

            return true;
        }

        private static Client CreateClient(ClientDto dto)
        {
            var client = new Client
            {
                ClientType = dto.ClientType,
                Phone = dto.Phone.Trim()
            };

            ApplyClientDetails(client, dto);

            return client;
        }

        private static void ApplyClientDetails(Client client, ClientDto dto)
        {
            if (dto.ClientType == PhysicalClientType)
            {
                var p = client.PhysicalClient ??= new PhysicalClient();
                p.Surname = dto.PhysicalClient!.Surname.Trim();
                p.Name = dto.PhysicalClient.Name.Trim();
                p.Patronymic = dto.PhysicalClient.Patronymic?.Trim();
            }
            else if (dto.ClientType == LegalClientType)
            {
                var l = client.LegalClient ??= new LegalClient();
                l.CompanyName = dto.LegalClient!.CompanyName.Trim();
                l.Inn = dto.LegalClient.Inn.Trim();
                l.Kpp = dto.LegalClient.Kpp?.Trim();
                l.SurnameContactPerson = dto.LegalClient.SurnameContactPerson.Trim();
                l.NameContactPerson = dto.LegalClient.NameContactPerson.Trim();
                l.PatronymicContactPerson = dto.LegalClient.PatronymicContactPerson?.Trim();
            }
        }

        private static FurnitureConfiguration CreateConfiguration(ConfigurationDto dto)
        {
            var configuration = new FurnitureConfiguration
            {
                IdModel = dto.IdModel,
                IdFabricCategory = dto.IdFabricCategory,
                IdTransformationMechanism = dto.IdTransformationMechanism,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
                Color = dto.Color.Trim()
            };

            foreach (var moduleId in dto.StorageModules.Distinct())
            {
                configuration.FurnitureConfigurationStorageModules.Add(
                    new FurnitureConfigurationStorageModule
                    {
                        IdStorageModule = moduleId
                    });
            }

            return configuration;
        }

        private static void ApplyDelivery(Order order, OrderDto dto)
        {
            order.DeliveryCity = dto.DeliveryCity.Trim();
            order.DeliveryStreet = dto.DeliveryStreet.Trim();
            order.DeliveryBuilding = dto.DeliveryBuilding.Trim();
            order.DeliveryApartment = dto.DeliveryApartment?.Trim();
        }

        private async Task<decimal> CalculateTotalAsync(IEnumerable<OrderItemConfigurationDto> items)
        {
            var models = (await _orderRepository.GetModelsAsync())
                .ToDictionary(m => m.IdModel, m => m.BasePrice);

            var fabrics = (await _orderRepository.GetFabricCategoriesAsync())
                .ToDictionary(f => f.IdFabricCategory, f => f.PriceMarkup);

            var mechanisms = (await _orderRepository.GetTransformationMechanismsAsync())
                .ToDictionary(m => m.IdTransformationMechanism, m => m.PriceMarkup);

            var modules = (await _orderRepository.GetStorageModulesAsync())
                .ToDictionary(m => m.IdStorageModule, m => m.BasePrice);

            decimal total = 0;

            foreach (var item in items)
            {
                var config = item.Configuration;

                if (!models.TryGetValue(config.IdModel, out var modelPrice))
                    throw new ArgumentException($"Модель {config.IdModel} не найдена.");

                if (!fabrics.TryGetValue(config.IdFabricCategory, out var fabricMarkup))
                    throw new ArgumentException($"Категория ткани {config.IdFabricCategory} не найдена.");

                if (!mechanisms.TryGetValue(config.IdTransformationMechanism, out var mechanismMarkup))
                    throw new ArgumentException( $"Механизм трансформации {config.IdTransformationMechanism} не найден.");

                decimal modulesPrice = 0;

                foreach (var moduleId in config.StorageModules.Distinct())
                {
                    if (!modules.TryGetValue(moduleId, out var modulePrice))
                        throw new ArgumentException(
                            $"Модуль хранения {moduleId} не найден.");

                    modulesPrice += modulePrice;
                }

                total += (modelPrice + fabricMarkup + mechanismMarkup + modulesPrice) * item.Quantity;
            }

            return total;
        }

        private static void Validate(OrderDto order)
        {
            var client = order.Client
                ?? throw new ArgumentException("Не указан клиент.");

            if (client.ClientType != PhysicalClientType &&
                client.ClientType != LegalClientType)
            {
                throw new ArgumentException("Неизвестный тип клиента.");
            }

            if (string.IsNullOrWhiteSpace(client.Phone))
                throw new ArgumentException("Не указан телефон клиента.");

            if (client.ClientType == PhysicalClientType)
            {
                var physical = client.PhysicalClient
                    ?? throw new ArgumentException("Не указаны данные физического лица.");

                if (string.IsNullOrWhiteSpace(physical.Surname) ||
                    string.IsNullOrWhiteSpace(physical.Name))
                {
                    throw new ArgumentException("Не указаны фамилия и имя клиента.");
                }
            }
            else
            {
                var legal = client.LegalClient
                    ?? throw new ArgumentException("Не указаны данные юридического лица.");

                if (string.IsNullOrWhiteSpace(legal.CompanyName) ||
                    string.IsNullOrWhiteSpace(legal.Inn) ||
                    string.IsNullOrWhiteSpace(legal.SurnameContactPerson) ||
                    string.IsNullOrWhiteSpace(legal.NameContactPerson))
                {
                    throw new ArgumentException("Не заполнены данные юридического лица.");
                }

                if (legal.Inn.Trim().Length != 10 ||
                    !legal.Inn.Trim().All(char.IsAsciiDigit))
                {
                    throw new ArgumentException("ИНН должен состоять из 10 цифр.");
                }

                if (!string.IsNullOrWhiteSpace(legal.Kpp) &&
                    (legal.Kpp.Trim().Length != 9 ||
                     !legal.Kpp.Trim().All(char.IsAsciiDigit)))
                {
                    throw new ArgumentException("КПП должен состоять из 9 цифр.");
                }
            }

            if (string.IsNullOrWhiteSpace(order.DeliveryCity) ||
                string.IsNullOrWhiteSpace(order.DeliveryStreet) ||
                string.IsNullOrWhiteSpace(order.DeliveryBuilding))
            {
                throw new ArgumentException("Не заполнен адрес доставки.");
            }

            if (order.OrderItem is null || order.OrderItem.Count == 0)
                throw new ArgumentException("В заказе нет позиций.");

            foreach (var item in order.OrderItem)
            {
                if (item is null || item.Configuration is null)
                    throw new ArgumentException("Некорректная позиция заказа.");

                var config = item.Configuration;

                if (item.Quantity <= 0)
                    throw new ArgumentException("Количество должно быть больше нуля.");

                if (config.Length <= 0 || config.Width <= 0 || config.Height <= 0)
                    throw new ArgumentException("Размеры должны быть больше нуля.");

                if (string.IsNullOrWhiteSpace(config.Color))
                    throw new ArgumentException("Не указан цвет.");

                if (config.IdModel <= 0 ||
                    config.IdFabricCategory <= 0 ||
                    config.IdTransformationMechanism <= 0)
                {
                    throw new ArgumentException("Не выбраны все параметры конфигурации.");
                }
            }
        }
    }
}
