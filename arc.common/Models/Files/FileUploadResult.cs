namespace arc.common.Models.Files;
public class FileUploadResult
{
    /// <summary>Database identifier for fileattachments row (0 when not yet persisted).</summary>
    public int Id { get; set; }
    /// <summary>Relative path from storage root including date/category folders (e.g., 2025/10/15/abcd1234.png).</summary>
    public string RelativePath { get; set; } = string.Empty;

    /// <summary>Stored filename (generated unique name with extension).</summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>Original filename provided by the client.</summary>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>MIME content type.</summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>File size in bytes.</summary>
    public long FileSize { get; set; }

    /// <summary>Optional SHA-256 hash of file contents for integrity/dedup.</summary>
    public string Sha256Hash { get; set; } = string.Empty;
}
