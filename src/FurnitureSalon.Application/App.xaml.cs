using FurnitureSalon.Application.Shared;
using FurnitureSalon.Application.ViewModels;
using FurnitureSalon.Application.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FurnitureSalon.Applications;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ServiceCollection services = new();

        services.AddTransient<LoginWindow>();
        services.AddTransient<LoginViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<ConfigWindow>();
        services.AddTransient<ConfigViewModel>();
        services.AddTransient<IWindowService, WindowService>();
        services.AddSingleton<ICurrentUser, CurrentUser>();
        services.AddSingleton<IConfigHelper, ConfigHelper>();

        services.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7298/");
        });

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        var startupWindow = serviceProvider.GetRequiredService<LoginWindow>();
        startupWindow.Show();
    }
}