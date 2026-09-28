namespace arc.domain.Configuration.ReportsConfig;

/// <summary>
/// Represents the configuration of an image in a report.
/// </summary>
public class ImageConfig
{
    /// <summary>
    /// The X coordinate of where to draw the image.
    /// </summary>
    public decimal X { get; set; }

    /// <summary>
    /// The Y coordinate of where to draw the image.
    /// </summary>
    public decimal Y { get; set; }

    /// <summary>
    /// Label of the image. Used for alt descriptions.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Name of the image.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Base64 encoded image data.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// The width of the image.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// The height of the image.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// The format of the image given as the file extension.
    /// e.g. png, jpg, jpeg, bmp, gif, etc.
    /// </summary>
    public string Format { get; set; }
}
