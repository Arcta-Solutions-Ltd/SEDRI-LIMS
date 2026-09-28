namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents an image element within a report section, defining its position, size, and properties.
/// </summary>
public class ReportImageModel
{
    /// <summary>
    /// Gets or sets the X coordinate position of the image in the report layout.
    /// </summary>
    public decimal X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate position of the image in the report layout.
    /// </summary>
    public decimal Y { get; set; }

    /// <summary>
    /// Gets or sets the label text associated with this image.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the name of the image used for identification and reference.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value or data source reference for this image.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the width of the image in the report layout.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the image in the report layout.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the format specification for the image (e.g., file extension, encoding).
    /// </summary>
    public string Format { get; set; }
}
