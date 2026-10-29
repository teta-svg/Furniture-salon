namespace FurnitureSalon.Application.Shared
{
    public class ConfigHelper : IConfigHelper
    {
        public int? IdOrder { get; set; }
        public OrderEditorMode EditorMode { get; set; }
    }
}