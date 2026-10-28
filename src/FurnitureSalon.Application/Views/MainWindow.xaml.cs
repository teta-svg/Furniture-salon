using FurnitureSalon.Application.ViewModels;
using System.Windows;

namespace FurnitureSalon.Applications;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        Loaded += async (_, _) => await viewModel.LoadOrdersCommand.ExecuteAsync(null);
    }
}