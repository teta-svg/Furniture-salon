using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureSalon.Application.Shared;
using FurnitureSalon.Applications;
using FurnitureSalon.Core.DTO.Login;
using System.Net.Http;

namespace FurnitureSalon.Application.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiClient _apiClient;
        private readonly IWindowService _windowService;

        public LoginViewModel(ApiClient apiClient, IWindowService windowService)
        {
            _apiClient = apiClient;
            _windowService = windowService;
        }

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorText = string.Empty;

        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorText = "Введите логин.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorText = "Введите пароль.";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private async Task LoginClick()
        {
            ErrorText = string.Empty;

            if (!Validation())
            {
                return;
            }

            var request = new LoginRequest
            {
                Login = Login,
                Password = Password
            };

            try
            {
                var response =
                    await _apiClient.PostAsync<LoginRequest, LoginResponse>("api/Auth/login", request);

                if (response is null || !response.IsSuccess)
                {
                    ErrorText = "Неверный логин или пароль.";
                    return;
                }

                CurrentUser.Set(response);

                ErrorText = string.Empty;

                _windowService.Show<MainWindow>(closeCurrent: true);
            }
            catch (HttpRequestException)
            {
                ErrorText = "Не удалось связаться с сервером.";
            }
            catch (TaskCanceledException)
            {
                ErrorText = "Время ожидания ответа истекло.";
            }
            catch (Exception ex)
            {
                ErrorText = $"Ошибка при попытке входа: {ex.Message}";
            }
        }
    }
}