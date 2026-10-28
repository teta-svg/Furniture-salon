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

        services.AddHttpClient();

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        var startupWindow = serviceProvider.GetRequiredService<LoginWindow>();
        startupWindow.Show();
    }
}