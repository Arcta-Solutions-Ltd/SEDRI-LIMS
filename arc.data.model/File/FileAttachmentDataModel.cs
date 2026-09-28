namespace arc.data.model.File;

/// <summary>
/// Represents the fields in the fileattachment table in the database.
/// </summary>
public class FileAttachmentDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the file path where the attachment is stored.
    /// </summary>
    public string? Filepath { get; set; }

    /// <summary>
    /// Gets or sets the filename of the attachment.
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the original filename of the attachment.
    /// </summary>
    public string? Originalfilename { get; set; }

    /// <summary>
    /// Gets or sets the content type of the attachment.
    /// </summary>
    public string? Contenttype { get; set; }

    /// <summary>
    /// Gets or sets the size of the file in bytes.
    /// </summary>
    public int Filesize { get; set; }

    /// <summary>
    /// Gets or sets the SHA-256 hash of the file.
    /// </summary>
    public string? Sha256Hash { get; set; }

    /// <summary>
    /// Gets or sets whether the file attachment is active.
    /// </summary>
    public bool IsActive { get; set; }
}
