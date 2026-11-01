using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureSalon.Core.DTO.Order;
using System.Collections.ObjectModel;

namespace FurnitureSalon.Application.ViewModels
{
    public partial class OrderItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _displayName;

        [ObservableProperty]
        private DateTime? _plannedReadyDate = DateTime.Today.AddDays(30);

        [ObservableProperty]
        private string _currentStatus = "Принят";

        [ObservableProperty]
        private IReadOnlyList<DictionaryItem> _models = [];

        [ObservableProperty]
        private IReadOnlyList<DictionaryItem> _fabricCategories = [];

        [ObservableProperty]
        private IReadOnlyList<DictionaryItem> _mechanisms = [];

        public ObservableCollection<StorageModuleOption> StorageModules { get; } = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private DictionaryItem? _selectedModel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private DictionaryItem? _selectedFabric;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private DictionaryItem? _selectedMechanism;

        [ObservableProperty]
        private string _color = "Бежевый";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private string _lengthText = "200";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private string _widthText = "90";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private string _heightText = "80";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private string _quantityText = "1";

        public int Quantity => int.TryParse(QuantityText, out var result) ? result : 0;

        public decimal TotalPrice
        {
            get
            {

                return 0;
            }
        }
    }
}