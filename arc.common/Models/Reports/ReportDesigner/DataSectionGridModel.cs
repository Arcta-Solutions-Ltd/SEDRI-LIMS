namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a grid definition within a data section, providing the structure
/// for grids that can be used in report sections.
/// </summary>
public class DataSectionGridModel
{
    /// <summary>
    /// Gets or sets the name of the grid used for identification and reference.
    /// </summary>
    public string Name {  get; set; }

    /// <summary>
    /// Gets or sets the data source or configuration for the grid content.
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Gets or sets the description shown in the designer when choosing a grid.
    /// </summary>
    public string Description { get; set; }
}
