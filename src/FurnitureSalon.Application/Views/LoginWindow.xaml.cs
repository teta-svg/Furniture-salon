using FurnitureSalon.Application.ViewModels;
using System.Windows;

namespace FurnitureSalon.Application.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
