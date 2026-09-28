namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the alerttype table in the database.
    /// </summary>
    public class AlertTypeDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the colour used to display the alert.
        /// </summary>
        public string? Colour { get; set; }
        /// <summary>
        /// Gets or sets the name of the alert.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the alerttype table. This links to the alerttype list in the listitem table.
        /// </summary>
        public int AlertCategoryId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the alerttype table denoting the screen position of the alert. This links to the alertposition list in the listitem table.
        /// </summary>

        public int PositionId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the alerttype table denoting the report position of the alert. This links to the alertposition list in the listitem table.
        /// </summary>
        public int ReportPositionId { get; set; }
    }
}
