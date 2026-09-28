using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a data section definition containing labeled fields and grid-based data entries.
/// </summary>
public class DataSectionDefinitionModel
{
    /// <summary>
    /// Gets or sets the internal identifier for the section.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the display title shown for the section.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the list of labeled data fields associated with this section.
    /// </summary>
    public List<DataSectionFieldModel> Fields { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of grid-based data entries associated with this section.
    /// </summary>
    public List<DataSectionGridModel> Grids { get; set; } = [];
}
