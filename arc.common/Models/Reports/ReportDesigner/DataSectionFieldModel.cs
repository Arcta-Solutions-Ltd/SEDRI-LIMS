namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a field definition within a data section, providing the basic structure
/// for fields that can be used in report sections.
/// </summary>
public class DataSectionFieldModel
{
    /// <summary>
    /// Gets or sets the label text that will be displayed for this field.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the value or data source reference for this field.
    /// </summary>
    public string Value { get; set; }
}
