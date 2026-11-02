using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Models;
using FurnitureSalon.Api.Services;
using FurnitureSalon.Core.DTO.Order;
using Moq;

namespace FurnitureSalon.xUnit
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
        }

        private static Order CreateOrder(int id, string itemStatus, string surname = "Иванов")
        {
            return new Order
            {
                IdOrder = id,
                IdEmployee = 1,
                CreationDate = DateOnly.FromDateTime(DateTime.Today),
                TotalPrice = 31000,
                DeliveryCity = "Москва",
                DeliveryStreet = "Ленина",
                DeliveryBuilding = "1",
                IdClientNavigation = new Client
                {
                    ClientType = "Физическое лицо",
                    Phone = "+79990000000",
                    PhysicalClient = new PhysicalClient
                    {
                        Surname = surname,
                        Name = "Иван"
                    }
                },
                OrderItems =
                {
                    new OrderItem
                    {
                        IdOrderItem = id,
                        Quantity = 2,
                        PlannedReadyDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
                        CurrentStatus = itemStatus,
                        IdFurnitureConfigurationNavigation = new FurnitureConfiguration
                        {
                            IdModel = 1,
                            IdFabricCategory = 1,
                            IdTransformationMechanism = 1,
                            Length = 200,
                            Width = 90,
                            Height = 80,
                            Color = "Бежевый",
                            IdModelNavigation = new Model { IdModel = 1, ModelName = "Верона" }
                        }
                    }
                }
            };
        }

        private static OrderDto CreateValidOrderDto(int idOrderItem = 0)
        {
            return new OrderDto
            {
                IdEmployee = 1,
                Client = new ClientDto
                {
                    ClientType = "Физическое лицо",
                    Phone = "+79990000000",
                    PhysicalClient = new PhysicalClientDto
                    {
                        Surname = "Иванов",
                        Name = "Иван"
                    }
                },
                DeliveryCity = "Москва",
                DeliveryStreet = "Ленина",
                DeliveryBuilding = "1",
                OrderItem =
                [
                    new OrderItemConfigurationDto
                    {
                        IdOrderItem = idOrderItem,
                        Quantity = 2,
                        PlannedReadyDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
                        Configuration = new ConfigurationDto
                        {
                            IdModel = 1,
                            IdFabricCategory = 1,
                            IdTransformationMechanism = 1,
                            StorageModules = [1],
                            Length = 200,
                            Width = 90,
                            Height = 80,
                            Color = "Бежевый"
                        }
                    }
                ]
            };
        }

        private void SetupDictionaries()
        {
            _orderRepositoryMock
                .Setup(r => r.GetModelsAsync())
                .ReturnsAsync([new Model { IdModel = 1, ModelName = "Верона", BasePrice = 10000 }]);

            _orderRepositoryMock
                .Setup(r => r.GetFabricCategoriesAsync())
                .ReturnsAsync([new FabricCategory { IdFabricCategory = 1, Name = "Эконом", PriceMarkup = 2000 }]);

            _orderRepositoryMock
                .Setup(r => r.GetTransformationMechanismsAsync())
                .ReturnsAsync([new TransformationMechanism { IdTransformationMechanism = 1, Name = "Еврокнижка", PriceMarkup = 3000 }]);

            _orderRepositoryMock
                .Setup(r => r.GetStorageModulesAsync())
                .ReturnsAsync([new StorageModule { IdStorageModule = 1, ModuleName = "Ящик", BasePrice = 500 }]);
        }

        [Fact]
        public async Task GetAllOrdersAsync_OrdersExist_ReturnsAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                CreateOrder(1, "Готов"),
                CreateOrder(2, "Ожидает производства", "Петров")
            };

            _orderRepositoryMock
                .Setup(r => r.GetOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Иванов И.", result[0].ClientName);
            Assert.Equal("Верона", result[0].OrderItems[0].ModelName);
        }

        [Fact]
        public async Task GetOrderByIdAsync_OrderExists_ReturnsOrderWithData()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(1))
                .ReturnsAsync(CreateOrder(1, "Готов"));

            // Act
            var result = await _orderService.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
            Assert.Equal("Москва", result.Order.DeliveryCity);
            Assert.Equal("Иванов", result.Order.Client.PhysicalClient?.Surname);
            Assert.Single(result.Order.OrderItem);
        }

        [Fact]
        public async Task GetOrderByIdAsync_OrderNotFound_ReturnsNull()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(99))
                .ReturnsAsync((Order?)null);

            // Act
            var result = await _orderService.GetOrderByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateOrderAsync_ValidData_SavesOrderWithCalculatedTotal()
        {
            // Arrange
            SetupDictionaries();

            Order? savedOrder = null;

            _orderRepositoryMock
                .Setup(r => r.CreateOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(o => savedOrder = o);

            var request = new CreateOrderRequest { Order = CreateValidOrderDto() };

            // Act
            await _orderService.CreateOrderAsync(request);

            // Assert
            _orderRepositoryMock.Verify(r => r.CreateOrderAsync(It.IsAny<Order>()), Times.Once);
            Assert.NotNull(savedOrder);
            Assert.Equal(31000m, savedOrder.TotalPrice);
            Assert.Single(savedOrder.OrderItems);
            Assert.Equal("Ожидает производства", savedOrder.OrderItems.First().CurrentStatus);
        }

        [Fact]
        public async Task CreateOrderAsync_EmptyPhone_ThrowsAndDoesNotSave()
        {
            // Arrange
            var dto = CreateValidOrderDto();
            dto.Client.Phone = "";

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _orderService.CreateOrderAsync(new CreateOrderRequest { Order = dto }));

            _orderRepositoryMock.Verify(r => r.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrderAsync_NoItems_ThrowsAndDoesNotSave()
        {
            // Arrange
            var dto = CreateValidOrderDto();
            dto.OrderItem = [];

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _orderService.CreateOrderAsync(new CreateOrderRequest { Order = dto }));

            _orderRepositoryMock.Verify(r => r.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrderAsync_EmptyDeliveryCity_ThrowsAndDoesNotSave()
        {
            // Arrange
            var dto = CreateValidOrderDto();
            dto.DeliveryCity = "";

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _orderService.CreateOrderAsync(new CreateOrderRequest { Order = dto }));

            _orderRepositoryMock.Verify(r => r.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }


        [Fact]
        public async Task UpdateOrderAsync_ValidData_UpdatesOrderAndReturnsTrue()
        {
            // Arrange
            SetupDictionaries();

            var order = CreateOrder(1, "Ожидает производства");

            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(1))
                .ReturnsAsync(order);

            var dto = CreateValidOrderDto(idOrderItem: 1);
            dto.DeliveryCity = "Казань";
            dto.OrderItem[0].Quantity = 3;

            // Act
            var result = await _orderService.UpdateOrderAsync(1, new EditOrderRequest { OrderId = 1, Order = dto });

            // Assert
            Assert.True(result);
            Assert.Equal("Казань", order.DeliveryCity);
            Assert.Equal(3, order.OrderItems.First().Quantity);
            Assert.Equal(46500m, order.TotalPrice); // 15500 * 3

            _orderRepositoryMock.Verify(
                r => r.UpdateOrderAsync(order, It.IsAny<IReadOnlyCollection<OrderItem>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_EmptyPhone_ThrowsAndDoesNotSave()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(1))
                .ReturnsAsync(CreateOrder(1, "Ожидает производства"));

            var dto = CreateValidOrderDto(idOrderItem: 1);
            dto.Client.Phone = "";

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _orderService.UpdateOrderAsync(1, new EditOrderRequest { OrderId = 1, Order = dto }));

            _orderRepositoryMock.Verify(
                r => r.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<IReadOnlyCollection<OrderItem>>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateOrderAsync_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(99))
                .ReturnsAsync((Order?)null);

            // Act
            var result = await _orderService.UpdateOrderAsync(99, new EditOrderRequest { Order = CreateValidOrderDto() });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SearchOrdersAsync_MatchingText_ReturnsMatchingOrders()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.SearchOrdersAsync("Иванов"))
                .ReturnsAsync(new List<Order> { CreateOrder(1, "Готов") });

            var request = new OrderSearchRequest { SearchText = "Иванов" };

            // Act
            var result = await _orderService.SearchOrdersAsync(request);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].IdOrder);
        }

        [Fact]
        public async Task SearchOrdersAsync_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.SearchOrdersAsync("несуществующий"))
                .ReturnsAsync(new List<Order>());

            var request = new OrderSearchRequest { SearchText = "несуществующий" };

            // Act
            var result = await _orderService.SearchOrdersAsync(request);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchOrdersAsync_StatusFilter_ReturnsOnlyOrdersWithStatus()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.SearchOrdersAsync(It.IsAny<string?>()))
                .ReturnsAsync(new List<Order>
                {
                    CreateOrder(1, "Готов"),
                    CreateOrder(2, "Ожидает производства", "Петров")
                });

            var request = new OrderSearchRequest { Status = "Готов" };

            // Act
            var result = await _orderService.SearchOrdersAsync(request);

            // Assert
            Assert.Single(result);
            Assert.Equal("Готов", result[0].CurrentStatus);
        }

        [Fact]
        public async Task SearchOrdersAsync_EmptyStatus_ReturnsAllOrders()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.SearchOrdersAsync(It.IsAny<string?>()))
                .ReturnsAsync(new List<Order>
                {
                    CreateOrder(1, "Готов"),
                    CreateOrder(2, "Ожидает производства", "Петров")
                });

            var request = new OrderSearchRequest { Status = null };

            // Act
            var result = await _orderService.SearchOrdersAsync(request);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task SearchOrdersAsync_SearchAndStatus_ReturnsOrdersMatchingBoth()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(r => r.SearchOrdersAsync("Иванов"))
                .ReturnsAsync(new List<Order>
                {
                    CreateOrder(1, "Готов"),
                    CreateOrder(2, "Ожидает производства")
                });

            var request = new OrderSearchRequest { SearchText = "Иванов", Status = "Готов" };

            // Act
            var result = await _orderService.SearchOrdersAsync(request);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].IdOrder);
        }
    }
}