namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a field within a report section, defining its display properties and layout.
/// </summary>
public class ReportSectionFieldModel
{
    /// <summary>
    /// Gets or sets the label text that will be displayed for this field.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the value or data source reference for this field.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the display text for this field.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the column number where this field should be positioned.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Gets or sets the order in which this field should be displayed within its column.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this field represents an image.
    /// </summary>
    public bool Image { get; set; }

    /// <summary>
    /// Gets or sets the width of the field in the report layout.
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the field in the report layout.
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    /// Gets or sets the format string for displaying the field value.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the PDF value cell should render without a border.
    /// </summary>
    public bool NoBox { get; set; }
}
