namespace arc.common.Models.Lists
{
    public class ListByIdQueryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OptionName { get; set; }
        public string Enabled { get; set; }
        public bool InternalHierarchy { get; set; }
        public string InternalHierarchyParentOptionName { get; set; }

        /// <summary>
        /// When set, this list is a child table linked to the parent list id.
        /// </summary>
        public int? ParentListId { get; set; }
    }
}
