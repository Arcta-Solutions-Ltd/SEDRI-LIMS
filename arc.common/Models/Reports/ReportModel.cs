namespace arc.common.Models.Reports
{
    /// <summary>
    /// Represents the model for a report.
    /// </summary>
    public class ReportModel
    {
        /// <summary>
        /// Gets or sets the Id of the specimen to report on.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the data associated with the report.
        /// </summary>
        public SpecimenSelectorListModel Value { get; set; }
    }
}
