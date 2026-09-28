using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents the data section configuration that defines the data source and structure
/// for a report section, including available fields and grids.
/// </summary>
public class ReportDataSectionModel
{
    /// <summary>
    /// Gets or sets the name of the data section used for identification and reference.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the display title of the data section.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the list of fields that are available in this data section.
    /// </summary>
    public List<DataSectionFieldModel> Fields { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of grids that are available in this data section.
    /// </summary>
    public List<DataSectionGridModel> Grids { get; set; } = [];
}
