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
        private readonly ICurrentUser _currentUser;

        public MainWindowViewModel(ApiClient apiClient, IWindowService windowService, ICurrentUser currentUser)
        {
            _apiClient = apiClient;
            _windowService = windowService;
            _currentUser = currentUser;

            EmployeeName = $"{_currentUser.User?.Surname} {_currentUser.User?.Name.Trim()[0]}. " +
                $"{(!string.IsNullOrWhiteSpace(_currentUser.User?.Patronymic) 
                ? $"{_currentUser.User?.Patronymic.Trim()[0]}." : string.Empty)}".Trim();
            EmployeeInitial = $"{_currentUser.User?.Surname.Trim()[0]}";
            EmployeeRole = _currentUser.User?.EmployeeRole;
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

        [ObservableProperty]
        private List<string> statuses =
        [
            "Все",
            "В производстве",
            "Ожидает производства",
            "Готов",
            "Отменён",
            "Доставлен"
        ];

        [ObservableProperty]
        private string? employeeName;

        [ObservableProperty]
        private string? employeeRole;

        [ObservableProperty]
        private string? employeeInitial;

        private async Task ExecuteApiRequestAsync(Func<Task<List<OrderListResponse>?>> execute)
        {
            try
            {
                var response = await execute();

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
        private async Task LoadOrdersAsync()
        {
            await ExecuteApiRequestAsync(() =>_apiClient.GetAsync<List<OrderListResponse>>("api/orders"));
           
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            var request = new OrderSearchRequest()
            {
                SearchText = SearchText,
                Status = SelectedStatus == "Все" ? null : SelectedStatus
            };

            await ExecuteApiRequestAsync(() => _apiClient.PostAsync<OrderSearchRequest, List<OrderListResponse>>("api/search/orders", request));
        }

        [RelayCommand]
        private async Task CreateOrderAsync()
        {
            //await OpenOrderAsync(OrderEditorMode.Create, 0);
        }

        private bool HasSelectedOrder() => SelectedOrder is not null;

        [RelayCommand(CanExecute = nameof(HasSelectedOrder))]
        private async Task ViewOrderAsync()
        {
            //await OpenOrderAsync(OrderEditorMode.View, SelectedOrder!.IdOrder);
        }

        [RelayCommand(CanExecute = nameof(HasSelectedOrder))]
        private async Task EditOrderAsync()
        {
            //await OpenOrderAsync(OrderEditorMode.Edit, SelectedOrder!.IdOrder);
        }

        //private async Task OpenOrderAsync(OrderEditorMode mode)
        //{
        //    _windowService.ShowWindow<ConfigWindow>();

        //    if (true)
        //    {
        //        if (string.IsNullOrWhiteSpace(SearchText) && SelectedStatus == "Все")
        //            await LoadOrdersAsync();
        //        else
        //            await SearchAsync();
        //    }
        //}
    }
}
