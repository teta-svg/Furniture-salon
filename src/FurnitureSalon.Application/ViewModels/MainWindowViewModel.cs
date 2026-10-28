using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureSalon.Application.Shared;
using FurnitureSalon.Core.DTO.Order;
using System.Collections.ObjectModel;
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
    }
}
