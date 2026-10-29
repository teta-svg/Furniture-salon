using System.Windows;

namespace FurnitureSalon.Application.Shared
{
    public interface IWindowService
    {
        void Show<TWindow>(bool closeCurrent = false)
             where TWindow : Window;

        bool? ShowDialog<TWindow, TViewModel>(Action<TViewModel> configure)
            where TWindow : Window
            where TViewModel : class;
    }
}
