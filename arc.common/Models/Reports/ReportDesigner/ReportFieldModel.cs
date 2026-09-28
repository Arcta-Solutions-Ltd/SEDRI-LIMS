namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a field configuration within a report section format.
/// </summary>
public class ReportFieldModel
{
    /// <summary>
    /// Gets or sets the label of the field.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the value of the field.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the plain text value of the field.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets whether the field is an image.
    /// </summary>
    public bool Image { get; set; }

    /// <summary>
    /// Gets or sets the width of the field.
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the field.
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    /// Gets or sets the format of the field (e.g., file extension for images).
    /// </summary>
    public string Format { get; set; }
}
