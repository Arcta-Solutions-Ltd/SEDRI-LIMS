namespace arc.common.Models.Config
{
    /// <summary>
    /// A combobox or dropdown field on a form that can act as a parent for a child list field.
    /// </summary>
    public class PageListFieldOptionModel
    {
        public string FieldId { get; set; }
        public string Label { get; set; }
        public int ListId { get; set; }
        public string ListName { get; set; }
    }
}
