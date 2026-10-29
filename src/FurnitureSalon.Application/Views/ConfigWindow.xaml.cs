using FurnitureSalon.Application.ViewModels;
using System.Windows;

namespace FurnitureSalon.Application.Views
{
    public partial class ConfigWindow : Window
    {
        public ConfigWindow(ConfigViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            viewModel.CloseRequested += result => DialogResult = result;
        }
    }
}
