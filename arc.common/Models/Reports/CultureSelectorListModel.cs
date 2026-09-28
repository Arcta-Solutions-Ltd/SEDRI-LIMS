namespace arc.common.Models.Reports
{
    /// <summary>
    /// Represents the model for a culture selector list.
    /// </summary>
    public class CultureSelectorListModel
    {
        /// <summary>
        /// Gets or sets the ID of the culture.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the culture.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the culture should be printed on the report.
        /// </summary>
        public string PrintOnReport { get; set; }
    }
}
