using CommunityToolkit.Mvvm.ComponentModel;

public partial class ClientInfoViewModel : ObservableObject
{
    public static readonly string PhysicalClientType = "Физическое лицо";
    public static readonly string LegalClientType = "Юридическое лицо";

    [ObservableProperty] private List<string> _clientTypes = [PhysicalClientType, LegalClientType];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPhysicalClient))]
    [NotifyPropertyChangedFor(nameof(IsLegalClient))]
    private string _selectedClientType = PhysicalClientType;

    public bool IsPhysicalClient => SelectedClientType == PhysicalClientType;
    public bool IsLegalClient => SelectedClientType == LegalClientType;

    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _surname = string.Empty;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _patronymic = string.Empty;

    [ObservableProperty] private string _companyName = string.Empty;
    [ObservableProperty] private string _inn = string.Empty;
    [ObservableProperty] private string _kpp = string.Empty;
    [ObservableProperty] private string _contactSurname = string.Empty;
    [ObservableProperty] private string _contactName = string.Empty;
    [ObservableProperty] private string _contactPatronymic = string.Empty;
}
