using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureSalon.Application.Shared;
using FurnitureSalon.Application.Views;
using FurnitureSalon.Core.DTO.Order;
using System.Net.Http;
using System.Windows;

namespace FurnitureSalon.Application.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly ApiClient _apiClient;
        private readonly IWindowService _windowService;

        public MainWindowViewModel(ApiClient apiClient, IWindowService windowService)
        {
            _apiClient = apiClient;
            _windowService = windowService;
        }

        [ObservableProperty]
        private List<OrderListResponse> orders = [];

        [ObservableProperty]
        private string? searchText;

        [ObservableProperty]
        private string? selectedStatus = "Все";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ViewOrderCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditOrderCommand))]
        private OrderListResponse? selectedOrder;

        public List<string> Statuses { get; } =
            [
            "Все",
            "В производстве",
            "Ожидает производства",
            "Готов",
            "Отменён",
            "Доставлен"
            ];

        public string EmployeeName
        {
            get
            {
                var user = CurrentUser.User;
                if (user is null) return string.Empty;

                string initials = string.Concat(
                    new[] { user.Name, user.Patronymic }
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => $"{s.Trim()[0]}. "));

                return $"{user.Surname} {initials}".Trim();
            }
        }

        public string EmployeeRole => CurrentUser.User?.EmployeeRole ?? string.Empty;

        public string EmployeeInitial =>
            string.IsNullOrWhiteSpace(CurrentUser.User?.Surname)
                ? "?"
                : CurrentUser.User!.Surname.Trim()[0].ToString().ToUpper();

        [RelayCommand]
        private async Task LoadOrdersAsync()
        {
            try
            {
                var response = await _apiClient.GetAsync<List<OrderListResponse>>("api/orders");

                Orders = response ?? [];
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Не удалось связаться с сервером.");
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Время ожидания ответа истекло.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            var request = new OrderSearchRequest()
            {
                SearchText = SearchText,
                Status = SelectedStatus == "Все" ? null : SelectedStatus
            };

            try
            {
                var response = await _apiClient.PostAsync<OrderSearchRequest, List<OrderListResponse>>("api/search/orders", request);

                Orders = response ?? [];
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Не удалось связаться с сервером.");
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Время ожидания ответа истекло.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task CreateOrderAsync()
        {
            await OpenOrderAsync(OrderEditorMode.Create, 0);
        }

        private bool HasSelectedOrder() => SelectedOrder is not null;

        [RelayCommand(CanExecute = nameof(HasSelectedOrder))]
        private async Task ViewOrderAsync()
        {
            await OpenOrderAsync(OrderEditorMode.View, SelectedOrder!.IdOrder);
        }

        [RelayCommand(CanExecute = nameof(HasSelectedOrder))]
        private async Task EditOrderAsync()
        {
            await OpenOrderAsync(OrderEditorMode.Edit, SelectedOrder!.IdOrder);
        }

        private async Task OpenOrderAsync(OrderEditorMode mode, int orderId)
        {
            bool? saved = _windowService.ShowDialog<ConfigWindow, ConfigViewModel>(
                viewModel => viewModel.Initialize(mode, orderId));

            if (saved == true)
            {
                if (string.IsNullOrWhiteSpace(SearchText) && SelectedStatus == "Все")
                    await LoadOrdersAsync();
                else
                    await SearchAsync();
            }
        }
    }
}
