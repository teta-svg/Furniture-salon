namespace FurnitureSalon.Application.Shared
{
    public interface IConfigHelper
    {
        int? IdOrder { get; set; }
        OrderEditorMode EditorMode { get; set; }
    }
}
