using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FurnitureSalon.Application.Shared
{
    public class WindowService : IWindowService
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ShowWindow<TWindow>(bool closeCurrent = false) where TWindow : Window
        {
            var currentWindow = System.Windows.Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive);

            var nextWindow = _serviceProvider.GetRequiredService<TWindow>();

            if (closeCurrent && currentWindow != null)
            {
                nextWindow.Show();
                currentWindow.Close();
            }
            else if (!closeCurrent)
            {
                nextWindow.ShowDialog();
            }
        }
    }
}