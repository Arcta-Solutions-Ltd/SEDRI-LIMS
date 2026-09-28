using System;

namespace arc.common.Models.Files;

/// <summary>Encapsulates the result of reading a file for download.</summary>
public class FileReadResult
{
    /// <summary>File bytes.</summary>
    public byte[] Bytes { get; set; } = Array.Empty<byte>();

    /// <summary>Suggested download filename.</summary>
    public string DownloadName { get; set; } = string.Empty;

    /// <summary>MIME content type.</summary>
    public string ContentType { get; set; } = "application/octet-stream";
}
