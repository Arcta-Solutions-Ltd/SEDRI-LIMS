namespace arc.data.model.Lists
{
    /// <summary>
    /// Represents the fields in the list table in the database.
    /// </summary>
    public class ListDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the name of the list.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets which grouping the list belongs to.
        /// </summary>
        public string? Grouping { get; set; }
        /// <summary>
        /// Gets or sets foreign key from the list table which represents the parent list to the current list.
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// Gets or sets foreign key from the list table which represents the parent list to the current list.
        /// </summary>
        public bool? Common {  get; set; }
        /// <summary>
        /// Gets or sets the description of the list. This is what will be displayed in the UI.
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// Gets or sets whether the list has been soft-deleted.
        /// </summary>
        public bool? Deleted { get; set; }
    }
}
