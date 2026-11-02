using CommunityToolkit.Mvvm.ComponentModel;

public partial class DeliveryInfoViewModel : ObservableObject
{
    [ObservableProperty] private string _deliveryCity = string.Empty;
    [ObservableProperty] private string _deliveryStreet = string.Empty;
    [ObservableProperty] private string _deliveryBuilding = string.Empty;
    [ObservableProperty] private string _deliveryApartment = string.Empty;
}