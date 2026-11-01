using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureSalon.Core.DTO.Order;

namespace FurnitureSalon.Application.ViewModels
{
    public partial class StorageModuleOption : ObservableObject
    {
        public StorageModuleOption(DictionaryItem item)
        {
            Item = item;
        }
        public DictionaryItem Item { get; } = new DictionaryItem();

        [ObservableProperty]
        private bool _isSelected;
    }
}
