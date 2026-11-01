using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureSalon.Application.Shared;
using FurnitureSalon.Core.DTO.Order;
using System.Collections.ObjectModel;
using System.Net.Http;
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

        private OrderItemViewModel CreateRow(OrderItemEditor data)
        {
            var model = Models.FirstOrDefault(m => m.Id == data.IdModel);
            var fabric = FabricCategories.FirstOrDefault(f => f.Id == data.IdFabricCategory);
            var mechanism = Mechanisms.FirstOrDefault(m => m.Id == data.IdTransformationMechanism);
            var modules = StorageModules
                .Where(s => data.StorageModuleIds.Contains(s.Item.Id))
                .Select(s => s.Item)
                .ToList();

            decimal unitPrice = (model?.Price ?? 0) + (fabric?.Price ?? 0) + (mechanism?.Price ?? 0)
                                + modules.Sum(m => m.Price);

            string displayName =
                $"{model?.Name}, {fabric?.Name}, {mechanism?.Name}, {data.Color}, " +
                $"{data.Length}×{data.Width}×{data.Height} см";

            if (modules.Count > 0)
                displayName += $" + {string.Join(", ", modules.Select(m => m.Name))}";

            return new OrderItemRow(data, displayName, unitPrice * data.Quantity);
        }



        [RelayCommand]
        private void AddItem()
        {

            if (SelectedModel is null || SelectedFabric is null || SelectedMechanism is null)
            {
                MessageBox.Show("Выберите модель, категорию ткани и механизм трансформации.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Color))
            {
                MessageBox.Show("Укажите цвет.");
                return;
            }

            if (!TryParsePositive(LengthText, out int length)
                || !TryParsePositive(WidthText, out int width)
                || !TryParsePositive(HeightText, out int height))
            {
                MessageBox.Show("Размеры должны быть целыми положительными числами.");
                return;
            }

            if (!TryParsePositive(QuantityText, out int quantity))
            {
                MessageBox.Show("Количество должно быть целым числом больше нуля.");
                return;
            }

            if (PlannedDate is null)
            {
                MessageBox.Show("Выберите плановую дату готовности.");
                return;
            }

            var plannedDate = DateOnly.FromDateTime(PlannedDate.Value);

            if (_editingRow is null && plannedDate < DateOnly.FromDateTime(DateTime.Today))
            {
                MessageBox.Show("Плановая дата готовности не может быть в прошлом.");
                return;
            }

            var data = new OrderItemEditor
            {
                IdOrderItem = _editingRow?.Data.IdOrderItem ?? 0,
                IdModel = SelectedModel.Id,
                IdFabricCategory = SelectedFabric.Id,
                IdTransformationMechanism = SelectedMechanism.Id,
                Length = length,
                Width = width,
                Height = height,
                Color = Color.Trim(),
                StorageModuleIds = StorageModules.Where(s => s.IsSelected).Select(s => s.Item.Id).ToList(),
                Quantity = quantity,
                PlannedReadyDate = plannedDate,
                CurrentStatus = _editingRow?.Data.CurrentStatus ?? string.Empty
            };

            var row = CreateRow(data);

            if (_editingRow is null)
            {
                Items.Add(row);
            }
            else
            {
                Items[Items.IndexOf(_editingRow)] = row;
                EndItemEdit();
            }

            SelectedItemRow = row;
        }

        private bool HasSelectedItem() => SelectedItemRow is not null;


        [RelayCommand(CanExecute = nameof(HasSelectedItem))]
        private void EditItem()
        {
            var row = SelectedItemRow!;
            var data = row.Data;

            _editingRow = row;
            IsEditingItem = true;

            SelectedModel = Models.FirstOrDefault(m => m.Id == data.IdModel);
            SelectedFabric = FabricCategories.FirstOrDefault(f => f.Id == data.IdFabricCategory);
            SelectedMechanism = Mechanisms.FirstOrDefault(m => m.Id == data.IdTransformationMechanism);

            Color = data.Color;
            LengthText = data.Length.ToString();
            WidthText = data.Width.ToString();
            HeightText = data.Height.ToString();
            QuantityText = data.Quantity.ToString();
            PlannedDate = data.PlannedReadyDate.ToDateTime(TimeOnly.MinValue);

            foreach (var module in StorageModules)
                module.IsSelected = data.StorageModuleIds.Contains(module.Item.Id);
        }

        private void EndItemEdit()
        {
            _editingRow = null;
            IsEditingItem = false;
        }

        private static bool TryParsePositive(string? text, out int value)
        {
            return int.TryParse(text, out value) && value > 0;
        }

        private string? ValidateOrder()
        {
            if (string.IsNullOrWhiteSpace(Phone))
                return "Укажите телефон клиента.";

            if (IsPhysicalClient)
            {
                if (string.IsNullOrWhiteSpace(Surname) || string.IsNullOrWhiteSpace(Name))
                    return "Укажите фамилию и имя клиента.";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(CompanyName))
                    return "Укажите название компании.";

                if (Inn.Trim().Length != 10 || !Inn.Trim().All(char.IsAsciiDigit))
                    return "ИНН должен состоять из 10 цифр.";

                if (!string.IsNullOrWhiteSpace(Kpp)
                    && (Kpp.Trim().Length != 9 || !Kpp.Trim().All(char.IsAsciiDigit)))
                    return "КПП должен состоять из 9 цифр.";

                if (string.IsNullOrWhiteSpace(ContactSurname) || string.IsNullOrWhiteSpace(ContactName))
                    return "Укажите фамилию и имя контактного лица.";
            }

            if (string.IsNullOrWhiteSpace(DeliveryCity)
                || string.IsNullOrWhiteSpace(DeliveryStreet)
                || string.IsNullOrWhiteSpace(DeliveryBuilding))
                return "Заполните город, улицу и дом в адресе доставки.";

            if (Items.Count == 0)
                return "Добавьте в заказ хотя бы одну позицию.";

            return null;
        }

        private OrderEditor BuildRequest()
        {
            return new OrderEditor
            {
                IdOrder = (int)_orderId!,
                IdEmployee = _currentUser.User?.IdEmployee ?? 0,
                ClientType = SelectedClientType,
                Phone = Phone.Trim(),
                Surname = Surname,
                Name = Name,
                Patronymic = Patronymic,
                CompanyName = CompanyName,
                Inn = Inn,
                Kpp = Kpp,
                ContactSurname = ContactSurname,
                ContactName = ContactName,
                ContactPatronymic = ContactPatronymic,
                DeliveryCity = DeliveryCity,
                DeliveryStreet = DeliveryStreet,
                DeliveryBuilding = DeliveryBuilding,
                DeliveryApartment = DeliveryApartment,
                Items = Items.Select(i => i.Data).ToList()
            };
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
                    await _apiClient.PostAsync<OrderEditor, CreateOrderResponse>("api/orders", request);
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
    }
}