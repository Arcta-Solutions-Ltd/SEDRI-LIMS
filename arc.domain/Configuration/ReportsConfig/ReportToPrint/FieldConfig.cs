namespace arc.domain.Configuration.ReportsConfig.ReportToPrint;

/// <summary>
/// Represents a field configuration for a report column.
/// </summary>
public class FieldConfig
{
    /// <summary>
    /// The label of the field.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// The value of the field.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// The plain text value of the field.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// If the field is an image.
    /// </summary>
    public bool Image { get; set; }

    /// <summary>
    /// The width of the field.
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    /// The height of the field.
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    /// If the field is an image then this represents the format of the image given as the file extension.
    /// e.g. png, jpg, jpeg, bmp, gif, etc.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// When true, the value cell renders without a border in the PDF.
    /// </summary>
    public bool NoBox { get; set; }
}
