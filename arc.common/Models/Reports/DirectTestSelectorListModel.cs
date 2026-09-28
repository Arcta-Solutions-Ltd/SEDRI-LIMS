namespace arc.common.Models.Reports
{
    /// <summary>
    /// Represents the model for a direct test selector list.
    /// </summary>
    public class DirectTestSelectorListModel
    {
        /// <summary>
        /// Gets or sets the ID of the direct test.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the direct test.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the direct test should be printed on the report.
        /// </summary>
        public string PrintOnReport { get; set; }
    }

}
