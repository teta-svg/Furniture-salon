using System.Windows;

namespace FurnitureSalon.Application.Shared
{
    public interface IWindowService
    {
        void ShowWindow<TWindow>(bool closeCurrent = false)
             where TWindow : Window;
    }
}
