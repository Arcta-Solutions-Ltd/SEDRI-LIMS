namespace arc.domain.Configuration.ViewConfig.ListViewConfig
{
    public class GridColumnsConfig
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public bool IsResizable { get; set; }
        public bool IsCollapsible { get; set; }
        public bool IsSorted { get; set; }
        public bool IsSortedDescending { get; set; }
        public bool Highlight { get; set; }

        /// <summary>
        /// When true, this column is hidden in hierarchy view (e.g. fullyqualifiedname when the tree structure shows hierarchy).
        /// </summary>
        public bool HideInHierarchy { get; set; }
    }
}
