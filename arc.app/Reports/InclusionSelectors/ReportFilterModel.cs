using arc.common.Models.Reports;

namespace arc.app.Reports.InclusionSelectors
{
    /// <summary>
    /// Represents the model for a report filter.
    /// </summary>
    public class ReportFilterModel
    {
        /// <summary>
        /// Gets or sets the report filter as a SpecimenSelectorListModel.
        /// </summary>
        public SpecimenSelectorListModel ReportFilter { get; set; }
    }
}
