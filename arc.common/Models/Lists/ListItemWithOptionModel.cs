namespace arc.common.Models.Lists
{
    public class ListItemWithOptionModel : ListItemModel
    {
        public string OptionName { get; set; }
        public bool InternalHierarchy { get; set; }
        public string InternalHierarchyParentOptionName { get; set; }
    }
}
