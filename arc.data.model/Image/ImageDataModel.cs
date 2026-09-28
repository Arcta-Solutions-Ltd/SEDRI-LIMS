namespace arc.data.model.Image;

/// <summary>
/// Data model for images
/// </summary>
public class ImageDataModel : IdAndDateBase
{
    /// <summary>
    /// The filename of the image
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A description or caption for the image
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The file path where the image is stored
    /// </summary>
    public int FileAttachmentId { get; set; }
}
