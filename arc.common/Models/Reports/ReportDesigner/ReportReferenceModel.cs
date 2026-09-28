using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner
{
    /// <summary>
    /// Represents a reference element that contains available data sections and fields.
    /// Categories can reference these by name to share the same data sources.
    /// </summary>
    public class ReportReferenceModel
    {
        /// <summary>
        /// Gets or sets the name of the reference element (e.g., "Main", "Organism").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of data section names that are available for this reference.
        /// </summary>
        public List<string> AvailableDataSections { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of fields that are available for use within this reference.
        /// </summary>
        public List<ReportAvailableFieldModel> AvailableFields { get; set; } = [];
    }
}
