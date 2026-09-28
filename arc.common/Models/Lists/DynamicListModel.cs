namespace arc.common.Models.Lists
{
    /// <summary>
    /// Represents the request from the UI for the contents of a list.
    /// </summary>
    public class DynamicListModel
    {
        /// <summary>
        /// Gets or sets the id of the list.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the list.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets whether to include fixed items when retrieving the list.
        /// </summary>
        public bool IncludeFixed { get; set; }
        /// <summary>
        /// Gets or sets whether the contents of the list should be translated.
        /// </summary>
        public bool Translate { get; set; }
        /// <summary>
        /// When true, returns only fixed hierarchy root items for lists with internal hierarchy enabled.
        /// </summary>
        public bool ParentNodesOnly { get; set; }
    }
}
