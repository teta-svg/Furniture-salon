using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureSalon.Application.Shared;
using FurnitureSalon.Core.DTO.Order;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace FurnitureSalon.Application.ViewModels
{
    public partial class ConfigViewModel : ObservableObject
    {
        private const string PhysicalClientType = "Физическое лицо";
        private const string LegalClientType = "Юридическое лицо";

        private readonly ApiClient _apiClient;
        private readonly ICurrentUser _currentUser;
        private readonly IConfigHelper _configHelper;

        private int? _orderId;
        private string _orderStatus = string.Empty;
        private OrderItemRow? _editingRow;

        public ConfigViewModel(ApiClient apiClient, ICurrentUser currentUser, IConfigHelper configHelper)
        {
            _apiClient = apiClient;
            _currentUser = currentUser;
            _configHelper = configHelper;

            _orderId = _configHelper.IdOrder;
            Mode = _configHelper.EditorMode;

            Items.CollectionChanged += (_, _) => RecalculateTotal();

            UpdateHeader();
        }

        public event Action<bool>? CloseRequested;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanEdit))]
        [NotifyPropertyChangedFor(nameof(IsClientTypeEditable))]
        [NotifyPropertyChangedFor(nameof(CloseButtonText))]
        private OrderEditorMode mode = OrderEditorMode.Create;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string subtitle = string.Empty;

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

        [ObservableProperty]
        private List<string> clientTypes = [PhysicalClientType, LegalClientType];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPhysicalClient))]
        [NotifyPropertyChangedFor(nameof(IsLegalClient))]
        private string _selectedClientType = PhysicalClientType;

        public bool IsPhysicalClient => SelectedClientType == PhysicalClientType;

        public bool IsLegalClient => SelectedClientType == LegalClientType;

        [ObservableProperty]
        private string _phone = string.Empty;

        [ObservableProperty]
        private string _surname = string.Empty;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _patronymic = string.Empty;


        [ObservableProperty]
        private string _companyName = string.Empty;

        [ObservableProperty]
        private string _inn = string.Empty;

        [ObservableProperty]
        private string _kpp = string.Empty;

        [ObservableProperty]
        private string _contactSurname = string.Empty;

        [ObservableProperty]
        private string _contactName = string.Empty;

        [ObservableProperty]
        private string _contactPatronymic = string.Empty;

        [ObservableProperty]
        private string _deliveryCity = string.Empty;

        [ObservableProperty]
        private string _deliveryStreet = string.Empty;

        [ObservableProperty]
        private string _deliveryBuilding = string.Empty;

        [ObservableProperty]
        private string _deliveryApartment = string.Empty;

        [ObservableProperty]
        private List<LookupItem> _models = [];

        [ObservableProperty]
        private List<LookupItem> _fabricCategories = [];

        [ObservableProperty]
        private List<LookupItem> _mechanisms = [];

        public ObservableCollection<StorageModuleOption> StorageModules { get; } = [];

        [ObservableProperty]
        private LookupItem? _selectedModel;

        [ObservableProperty]
        private LookupItem? _selectedFabric;

        [ObservableProperty]
        private LookupItem? _selectedMechanism;

        [ObservableProperty]
        private string _color = "Бежевый";

        [ObservableProperty]
        private string _lengthText = "200";

        [ObservableProperty]
        private string _widthText = "90";

        [ObservableProperty]
        private string _heightText = "80";

        [ObservableProperty]
        private string _quantityText = "1";

        [ObservableProperty]
        private DateTime? _plannedDate = DateTime.Today.AddDays(30);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AddItemButtonText))]
        private bool _isEditingItem;

        public string AddItemButtonText =>
            IsEditingItem ? "Сохранить изменения позиции" : "Добавить конфигурацию в заказ";

        partial void OnSelectedModelChanged(LookupItem? value) => UpdateConfigurationPrice();
        partial void OnSelectedFabricChanged(LookupItem? value) => UpdateConfigurationPrice();
        partial void OnSelectedMechanismChanged(LookupItem? value) => UpdateConfigurationPrice();
        partial void OnQuantityTextChanged(string value) => UpdateConfigurationPrice();



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

        private static string Money(decimal value) => $"{value:N2} ₽";

        private void UpdateConfigurationPrice()
        {
            if (SelectedModel is null || SelectedFabric is null || SelectedMechanism is null)
            {
                ModelPriceText = FabricPriceText = MechanismPriceText = "—";
                StoragePriceText = ConfigurationTotalText = ConfigurationQuantityTotalText = "—";
                return;
            }

            decimal modules = StorageModules.Where(m => m.IsSelected).Sum(m => m.Item.Price);
            decimal unitPrice = SelectedModel.Price + SelectedFabric.Price + SelectedMechanism.Price + modules;

            int quantity = int.TryParse(QuantityText, out int q) && q > 0 ? q : 0;

            ModelPriceText = Money(SelectedModel.Price);
            FabricPriceText = Money(SelectedFabric.Price);
            MechanismPriceText = Money(SelectedMechanism.Price);
            StoragePriceText = Money(modules);
            ConfigurationTotalText = Money(unitPrice);
            ConfigurationQuantityTotalText = Money(unitPrice * quantity);
        }



        public ObservableCollection<OrderItemRow> Items { get; } = [];

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditItemCommand))]
        [NotifyCanExecuteChangedFor(nameof(RemoveItemCommand))]
        private OrderItemRow? _selectedItemRow;

        [ObservableProperty]
        private string _totalText = "Итого: 0,00 ₽";

        private void RecalculateTotal()
        {
            TotalText = $"Итого: {Money(Items.Sum(i => i.TotalPrice))}";
        }




        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string _errorText = string.Empty;

        public bool HasError => !string.IsNullOrEmpty(ErrorText);

        private void ShowError(Exception ex)
        {
            ErrorText = ex switch
            {
                HttpRequestException http when http.StatusCode is null => "Не удалось связаться с сервером.",
                HttpRequestException http => http.Message,
                TaskCanceledException => "Время ожидания ответа истекло.",
                _ => $"Ошибка: {ex.Message}"
            };
        }




        [RelayCommand]
        private async Task LoadAsync()
        {
            ErrorText = string.Empty;

            try
            {
                var lookups = await _apiClient.GetAsync<OrderLookupsResponse>("api/orders/lookups")
                              ?? new OrderLookupsResponse();

                Models = lookups.Models;
                FabricCategories = lookups.FabricCategories;
                Mechanisms = lookups.TransformationMechanisms;

                StorageModules.Clear();

                foreach (var module in lookups.StorageModules)
                {
                    var option = new StorageModuleOption(module);
                    option.PropertyChanged += (_, _) => UpdateConfigurationPrice();
                    StorageModules.Add(option);
                }

                SelectedModel = Models.FirstOrDefault();
                SelectedFabric = FabricCategories.FirstOrDefault();
                SelectedMechanism = Mechanisms.FirstOrDefault();

                if (Mode == OrderEditorMode.Create)
                    return;

                var order = await _apiClient.GetAsync<OrderEditor>($"api/orders/{_orderId}");

                if (order is null)
                {
                    ErrorText = "Заказ не найден.";
                    return;
                }

                FillFromOrder(order);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void FillFromOrder(OrderEditor order)
        {
            SelectedClientType = order.ClientType;
            Phone = order.Phone;

            Surname = order.Surname ?? string.Empty;
            Name = order.Name ?? string.Empty;
            Patronymic = order.Patronymic ?? string.Empty;

            CompanyName = order.CompanyName ?? string.Empty;
            Inn = order.Inn ?? string.Empty;
            Kpp = order.Kpp ?? string.Empty;
            ContactSurname = order.ContactSurname ?? string.Empty;
            ContactName = order.ContactName ?? string.Empty;
            ContactPatronymic = order.ContactPatronymic ?? string.Empty;

            DeliveryCity = order.DeliveryCity;
            DeliveryStreet = order.DeliveryStreet;
            DeliveryBuilding = order.DeliveryBuilding;
            DeliveryApartment = order.DeliveryApartment ?? string.Empty;

            Items.Clear();

            foreach (var item in order.Items)
                Items.Add(CreateRow(item));

            _orderStatus = order.CurrentStatus;
            UpdateHeader();
        }

        private OrderItemRow CreateRow(OrderItemEditor data)
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
            ErrorText = string.Empty;

            if (SelectedModel is null || SelectedFabric is null || SelectedMechanism is null)
            {
                ErrorText = "Выберите модель, категорию ткани и механизм трансформации.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Color))
            {
                ErrorText = "Укажите цвет.";
                return;
            }

            if (!TryParsePositive(LengthText, out int length)
                || !TryParsePositive(WidthText, out int width)
                || !TryParsePositive(HeightText, out int height))
            {
                ErrorText = "Размеры должны быть целыми положительными числами.";
                return;
            }

            if (!TryParsePositive(QuantityText, out int quantity))
            {
                ErrorText = "Количество должно быть целым числом больше нуля.";
                return;
            }

            if (PlannedDate is null)
            {
                ErrorText = "Выберите плановую дату готовности.";
                return;
            }

            var plannedDate = DateOnly.FromDateTime(PlannedDate.Value);

            if (_editingRow is null && plannedDate < DateOnly.FromDateTime(DateTime.Today))
            {
                ErrorText = "Плановая дата готовности не может быть в прошлом.";
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

        [RelayCommand]
        private void CancelItemEdit()
        {
            EndItemEdit();
        }

        [RelayCommand(CanExecute = nameof(HasSelectedItem))]
        private void RemoveItem()
        {
            var row = SelectedItemRow!;

            if (ReferenceEquals(_editingRow, row))
                EndItemEdit();

            Items.Remove(row);
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
            ErrorText = string.Empty;

            string? error = ValidateOrder();

            if (error is not null)
            {
                ErrorText = error;
                return;
            }

            if (Mode == OrderEditorMode.Create && _currentUser.User is null)
            {
                ErrorText = "Не удалось определить сотрудника. Войдите в систему заново.";
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
                ShowError(ex);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            CloseRequested?.Invoke(false);
        }
    }
}