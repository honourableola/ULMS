namespace WebUI.Models.Components
{
    public class DatatableComponentModelColumn
    {
        public string FieldName { get; set; }
        public string Title { get; set; }
        public bool IsSortable { get; set; } = true;
        public bool ShouldAutoHide { get; set; } = true;
        public int Width { get; set; }
        public string Type { get; set; }
        public string CssClass { get; set; }
        public string TextAlign { get; set; }
        public string Template { get; set; }
        public string Format { get; set; }
    }
}