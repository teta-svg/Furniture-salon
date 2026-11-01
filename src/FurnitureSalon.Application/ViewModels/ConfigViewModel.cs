using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureSalon.Application.Shared;
using FurnitureSalon.Core.DTO.Order;
using System.Collections.ObjectModel;
using System.Windows;

namespace FurnitureSalon.Application.ViewModels
{
    public partial class ConfigViewModel : ObservableObject
    {
        private readonly ApiClient _apiClient;
        private readonly ICurrentUser _currentUser;
        private readonly IConfigHelper _configHelper;
        private int? _orderId;
        private string _orderStatus = string.Empty;
        private OrderItemViewModel? _editingRow;

        public ConfigViewModel(ApiClient apiClient, ICurrentUser currentUser, IConfigHelper configHelper)
        {
            _apiClient = apiClient;
            _currentUser = currentUser;
            _configHelper = configHelper;

            _orderId = _configHelper.IdOrder;
            Mode = _configHelper.EditorMode;

            UpdateHeader();
        }

        public event Action<bool>? CloseRequested;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanEdit))]
        [NotifyPropertyChangedFor(nameof(IsClientTypeEditable))]
        [NotifyPropertyChangedFor(nameof(CloseButtonText))]
        private OrderEditorMode _mode = OrderEditorMode.Create;

        partial void OnModeChanged(OrderEditorMode value) => UpdateHeader();

        [ObservableProperty] private string _title = string.Empty;
        [ObservableProperty] private string _subtitle = string.Empty;

        public bool CanEdit => Mode != OrderEditorMode.View;
        public bool IsClientTypeEditable => Mode == OrderEditorMode.Create;
        public string CloseButtonText => Mode == OrderEditorMode.View ? "Закрыть" : "Отмена";

        private void UpdateHeader()
        {
            switch (Mode)
            {
                case OrderEditorMode.View:
                    Title = $"Заказ № {_orderId}";
                    Subtitle = string.IsNullOrEmpty(_orderStatus)
                        ? "Просмотр заказа"
                        : $"Просмотр заказа. Статус: {_orderStatus}";
                    break;

                case OrderEditorMode.Edit:
                    Title = $"Редактирование заказа № {_orderId}";
                    Subtitle = "Измените данные и нажмите «Сохранить заказ»";
                    break;

                default:
                    Title = "Создание заказа";
                    Subtitle = "Заполните данные клиента, доставки и состав заказа";
                    break;
            }
        }
        
        public ObservableCollection<OrderItemViewModel> OrderItems { get; } = [];
        public OrderItemViewModel ItemEditor { get; } = new();
        public ClientInfoViewModel ClientInfo { get; } = new();
        public DeliveryInfoViewModel DeliveryInfo { get; } = new();

        #region поля для цены

        [ObservableProperty]
        private string _modelPriceText = "—";

        [ObservableProperty]
        private string _fabricPriceText = "—";

        [ObservableProperty]
        private string _mechanismPriceText = "—";

        [ObservableProperty]
        private string _storagePriceText = "—";

        [ObservableProperty]
        private string _configurationTotalText = "—";

        [ObservableProperty]
        private string _configurationQuantityTotalText = "—";

        #endregion


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AddItemButtonText))]
        private bool _isEditingItem;

        public string AddItemButtonText => IsEditingItem ? "Сохранить изменения позиции" : "Добавить конфигурацию в заказ";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditItemCommand))]
        private OrderItemViewModel? _selectedItem;

        [ObservableProperty]
        private string _totalText = "Итого: 0,00 ₽";


        [RelayCommand]
        private async Task LoadAsync()
        {
            try
            {
                var dict = await _apiClient.GetAsync<OrderDictionariesResponse>("api/orders/dict") ?? new OrderDictionariesResponse();

                var order = await _apiClient.GetAsync<OrderResponse>($"api/orders/{_orderId}");

                if (order is null)
                {
                    MessageBox.Show("Заказ не найден.");
                    return;
                }

                MapOrder(order);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при загрузке заказа: " + ex.Message);
            }
        }

        private void MapOrder(OrderResponse response)
        {
            var order = response.Order;

            MapClient(order.Client);

            MapDelivery(order);

            OrderItems.Clear();

            foreach (var item in order.OrderItem)
            {
                OrderItems.Add(MapOrderItem(item));
            }

            UpdateHeader();
        }

        private void MapClient(ClientDto client)
        {
            ClientInfo.SelectedClientType = client.ClientType ?? string.Empty;
            ClientInfo.Phone = client.Phone ?? string.Empty;

            if (client.PhysicalClient is not null)
            {
                ClientInfo.Surname = client.PhysicalClient.Surname ?? string.Empty;
                ClientInfo.Name = client.PhysicalClient.Name ?? string.Empty;
                ClientInfo.Patronymic = client.PhysicalClient.Patronymic ?? string.Empty;
            }

            if (client.LegalClient is not null)
            {
                ClientInfo.CompanyName = client.LegalClient.CompanyName ?? string.Empty;
                ClientInfo.Inn = client.LegalClient.Inn ?? string.Empty;
                ClientInfo.Kpp = client.LegalClient.Kpp ?? string.Empty;

                ClientInfo.ContactSurname =
                    client.LegalClient.SurnameContactPerson ?? string.Empty;

                ClientInfo.ContactName =
                    client.LegalClient.NameContactPerson ?? string.Empty;

                ClientInfo.ContactPatronymic =
                    client.LegalClient.PatronymicContactPerson ?? string.Empty;
            }
        }

        private void MapDelivery(OrderDto order)
        {
            DeliveryInfo.DeliveryCity = order.DeliveryCity ?? string.Empty;
            DeliveryInfo.DeliveryStreet = order.DeliveryStreet ?? string.Empty;
            DeliveryInfo.DeliveryBuilding = order.DeliveryBuilding ?? string.Empty;
            DeliveryInfo.DeliveryApartment = order.DeliveryApartment ?? string.Empty;
        }

        private OrderItemViewModel MapOrderItem(OrderItemConfigurationDto item)
        {
            var config = item.Configuration;

            var itemVm = new OrderItemViewModel
            {
                Color = config.Color ?? "Бежевый",

                LengthText = config.Length.ToString(),
                WidthText = config.Width.ToString(),
                HeightText = config.Height.ToString(),

                QuantityText = item.Quantity.ToString(),

                CurrentStatus = item.CurrentStatus ?? "Принят",
                PlannedReadyDate = item.PlannedReadyDate
            };

            itemVm.SelectedModel = itemVm.Models
                .FirstOrDefault(m => m.Id == config.IdModel);

            itemVm.DisplayName = itemVm.SelectedModel?.Name
                ?? $"Модель #{config.IdModel}";

            itemVm.SelectedFabric = itemVm.FabricCategories
                .FirstOrDefault(f => f.Id == config.IdFabricCategory);

            itemVm.SelectedMechanism = itemVm.Mechanisms
                .FirstOrDefault(m => m.Id == config.IdTransformationMechanism);

            var selectedIds = config.StorageModules ?? [];

            foreach (var module in itemVm.StorageModules)
            {
                module.IsSelected = module.Item is not null && selectedIds.Contains(module.Item.Id);
            }

            return itemVm;
        }

        [RelayCommand]
        private void AddItem()
        {
            var editor = ItemEditor;

            if (editor.SelectedModel is null ||
                editor.SelectedFabric is null ||
                editor.SelectedMechanism is null)
            {
                MessageBox.Show("Выберите модель, категорию ткани и механизм трансформации.");
                return;
            }

            if (string.IsNullOrWhiteSpace(editor.Color))
            {
                MessageBox.Show("Укажите цвет.");
                return;
            }

            if (!TryParsePositive(editor.LengthText, out int length) ||
                !TryParsePositive(editor.WidthText, out int width) ||
                !TryParsePositive(editor.HeightText, out int height))
            {
                MessageBox.Show("Размеры должны быть целыми положительными числами.");
                return;
            }

            if (!TryParsePositive(editor.QuantityText, out int quantity))
            {
                MessageBox.Show("Количество должно быть целым числом больше нуля.");
                return;
            }

            if (editor.PlannedReadyDate < DateOnly.FromDateTime(DateTime.Today))
            {
                MessageBox.Show("Плановая дата готовности не может быть в прошлом.");
                return;
            }

            var item = new OrderItemViewModel
            {
                Models = editor.Models,
                FabricCategories = editor.FabricCategories,
                Mechanisms = editor.Mechanisms,

                SelectedModel = editor.SelectedModel,
                SelectedFabric = editor.SelectedFabric,
                SelectedMechanism = editor.SelectedMechanism,

                DisplayName = editor.SelectedModel.Name,
                Color = editor.Color.Trim(),

                LengthText = length.ToString(),
                WidthText = width.ToString(),
                HeightText = height.ToString(),
                QuantityText = quantity.ToString(),

                PlannedReadyDate = editor.PlannedReadyDate,
                CurrentStatus = _editingRow?.CurrentStatus ?? "Принят"
            };

            foreach (var module in editor.StorageModules)
            {
                item.StorageModules.Add(
                    new StorageModuleOption(module.Item)
                    {
                        IsSelected = module.IsSelected
                    });
            }

            if (_editingRow is null)
            {
                OrderItems.Add(item);
            }
            else
            {
                int index = OrderItems.IndexOf(_editingRow);

                if (index >= 0)
                    OrderItems[index] = item;

                _editingRow = null;
                IsEditingItem = false;
            }

            SelectedItem = item;
        }


        private bool HasSelectedItem() => SelectedItem is not null;

        [RelayCommand(CanExecute = nameof(HasSelectedItem))]
        private void EditItem()
        {
            if (SelectedItem is null)
                return;

            _editingRow = SelectedItem;
            IsEditingItem = true;

            var editor = ItemEditor;
            var item = SelectedItem;

            editor.SelectedModel = item.SelectedModel;
            editor.SelectedFabric = item.SelectedFabric;
            editor.SelectedMechanism = item.SelectedMechanism;

            editor.Color = item.Color;
            editor.LengthText = item.LengthText;
            editor.WidthText = item.WidthText;
            editor.HeightText = item.HeightText;
            editor.QuantityText = item.QuantityText;
            editor.PlannedReadyDate = item.PlannedReadyDate;

            foreach (var module in editor.StorageModules)
            {
                var selectedModule = item.StorageModules
                    .FirstOrDefault(m => m.Item.Id == module.Item.Id);

                module.IsSelected = selectedModule?.IsSelected ?? false;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            string? error = ValidateOrder();

            if (error is not null)
            {
                MessageBox.Show(error);
                return;
            }

            if (Mode == OrderEditorMode.Create && _currentUser.User is null)
            {
                MessageBox.Show("Не удалось определить сотрудника. Войдите в систему заново.");
                return;
            }

            var request = BuildRequest();

            try
            {
                if (Mode == OrderEditorMode.Create)
                    await _apiClient.PutAsync("api/orders", request);
                else
                    await _apiClient.PutAsync($"api/orders/{_orderId}", request);

                CloseRequested?.Invoke(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении заказа: " + ex.Message);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            CloseRequested?.Invoke(false);
        }

        private CreateOrderRequest BuildRequest()
        {
            var order = new OrderDto
            {
                Client = new ClientDto
                {
                    ClientType = ClientInfo.SelectedClientType,
                    Phone = ClientInfo.Phone.Trim(),

                    PhysicalClient = ClientInfo.SelectedClientType == "Физическое лицо"
                        ? new PhysicalClientDto
                        {
                            Surname = ClientInfo.Surname.Trim(),
                            Name = ClientInfo.Name.Trim(),
                            Patronymic = ClientInfo.Patronymic
                        }
                        : null,

                    LegalClient = ClientInfo.SelectedClientType == "Юридическое лицо"
                        ? new LegalClientDto
                        {
                            CompanyName = ClientInfo.CompanyName.Trim(),
                            Inn = ClientInfo.Inn.Trim(),
                            Kpp = ClientInfo.Kpp,
                            SurnameContactPerson = ClientInfo.ContactSurname.Trim(),
                            NameContactPerson = ClientInfo.ContactName.Trim(),
                            PatronymicContactPerson = ClientInfo.ContactPatronymic
                        }
                        : null
                },

                IdEmployee = _currentUser.User!.IdEmployee,

                OrderItem = OrderItems.Select(item => new OrderItemConfigurationDto
                {
                    Configuration = new ConfigurationDto
                    {
                        IdModel = item.SelectedModel!.Id,
                        IdFabricCategory = item.SelectedFabric!.Id,
                        IdTransformationMechanism = item.SelectedMechanism!.Id,
                        StorageModules = item.StorageModules
                            .Where(module => module.IsSelected)
                            .Select(module => module.Item.Id)
                            .ToList(),

                        Length = int.Parse(item.LengthText),
                        Width = int.Parse(item.WidthText),
                        Height = int.Parse(item.HeightText),
                        Color = item.Color.Trim()
                    },

                    PlannedReadyDate = item.PlannedReadyDate,
                    Quantity = item.Quantity,
                    CurrentStatus = item.CurrentStatus
                }).ToList(),

                DeliveryCity = DeliveryInfo.DeliveryCity.Trim(),
                DeliveryStreet = DeliveryInfo.DeliveryStreet.Trim(),
                DeliveryBuilding = DeliveryInfo.DeliveryBuilding.Trim(),
                DeliveryApartment = DeliveryInfo.DeliveryApartment
            };

            return new CreateOrderRequest
            {
                Order = order
            };
        }
        private bool TryParsePositive(string? value, out int result)
        {
            return int.TryParse(value, out result) && result > 0;
        }
        private string? ValidateOrder()
        {
            if (string.IsNullOrWhiteSpace(ClientInfo.Phone))
                return "Укажите телефон клиента.";

            if (ClientInfo.IsPhysicalClient)
            {
                if (string.IsNullOrWhiteSpace(ClientInfo.Surname) || string.IsNullOrWhiteSpace(ClientInfo.Name))
                    return "Укажите фамилию и имя клиента.";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ClientInfo.CompanyName))
                    return "Укажите название компании.";

                if (ClientInfo.Inn.Trim().Length != 10 || !ClientInfo.Inn.Trim().All(char.IsAsciiDigit))
                    return "ИНН должен состоять из 10 цифр.";

                if (!string.IsNullOrWhiteSpace(ClientInfo.Kpp) && (ClientInfo.Kpp.Trim().Length != 9 ||
                    !ClientInfo.Kpp.Trim().All(char.IsAsciiDigit)))
                    return "КПП должен состоять из 9 цифр.";

                if (string.IsNullOrWhiteSpace(ClientInfo.ContactSurname) || string.IsNullOrWhiteSpace(ClientInfo.ContactName))
                    return "Укажите фамилию и имя контактного лица.";
            }

            if (string.IsNullOrWhiteSpace(DeliveryInfo.DeliveryCity) ||
                string.IsNullOrWhiteSpace(DeliveryInfo.DeliveryStreet) ||
                string.IsNullOrWhiteSpace(DeliveryInfo.DeliveryBuilding))
            {
                return "Заполните город, улицу и дом в адресе доставки.";
            }

            if (OrderItems.Count == 0)
                return "Добавьте в заказ хотя бы одну позицию.";

            return null;
        }
    }
}