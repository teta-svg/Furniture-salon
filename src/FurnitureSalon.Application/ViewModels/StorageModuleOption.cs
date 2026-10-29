using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Application.ViewModels
{
    public partial class StorageModuleOption : ObservableObject
    {
        public StorageModuleOption(LookupItem item)
        {
            Item = item;
        }

        public LookupItem Item { get; }

        public string Label => $"{Item.Name} (+{Item.Price:N0} ₽)";

        [ObservableProperty]
        private bool _isSelected;
    }
}
