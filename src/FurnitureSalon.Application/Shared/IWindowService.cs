using System.Windows;

namespace FurnitureSalon.Application.Shared
{
    public interface IWindowService
    {
        void Show<TWindow>(bool closeCurrent = false)
            where TWindow : Window;
    }
}
