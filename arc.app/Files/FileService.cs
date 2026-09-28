using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using arc.common.Models.Files;

namespace arc.app.Files;

/// <summary>Persists files to disk under yyyy/MM/dd and serves by fileattachments id.</summary>
public class FileService : IFileService
{
    private readonly string _storageRoot;

    /// <summary>Creates a new instance.</summary>
    /// <summary>
    /// Initializes a new instance of the <see cref="FileService"/> class.
    /// </summary>
    /// <param name="storageRoot">Absolute path to the storage root directory.</param>

    public FileService(string storageRoot)
    {
        _storageRoot = storageRoot ?? throw new ArgumentNullException(nameof(storageRoot));
    }

    /// <summary>
    /// Saves a file stream to disk using a date-based folder structure and returns metadata.
    /// </summary>
    /// <param name="content">Readable stream for the file contents.</param>
    /// <param name="originalFileName">Original filename to preserve extension.</param>
    /// <param name="contentType">MIME content type.</param>
    /// <param name="category">Top-level logical category under storage root.</param>
    /// <param name="timestampUtc">Timestamp to use for date folder resolution.</param>
    /// <param name="outputSubfolder">Optional subfolder relative to storage root. When set, path is outputSubfolder/yyyy/MM/dd/filename. Rejects paths containing ".." for security.</param>
    /// <returns>Upload metadata including relative path and content hash.</returns>
    public async Task<FileUploadResult> SaveFileAsync(Stream content, string originalFileName, string contentType, string category = "image", DateTime? timestampUtc = null, string? outputSubfolder = null)
    {
        if (content == null) throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrWhiteSpace(originalFileName)) throw new ArgumentException("Original file name is required", nameof(originalFileName));

        var now = (timestampUtc ?? DateTime.UtcNow);

        var extension = Path.GetExtension(originalFileName);
        var uniqueName = Guid.NewGuid().ToString("N") + extension;

        var relativePath = BuildRelativePath(category, now, uniqueName, outputSubfolder);
        var absolutePath = Path.Combine(_storageRoot, NormalizeSeparators(relativePath));

        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

        // Save to disk
        using (var fileStream = File.Create(absolutePath))
        {
            await content.CopyToAsync(fileStream);
        }

        // Compute SHA-256 (optional integrity/dedup)
        string sha256;
        content.Position = 0;
        using (var sha = SHA256.Create())
        {
            // Re-open the file to hash saved content to be safe
            using var fs = File.OpenRead(absolutePath);
            var hash = await sha.ComputeHashAsync(fs);
            sha256 = BytesToHex(hash);
        }

        var fileInfo = new FileInfo(absolutePath);

        return new FileUploadResult
        {
            RelativePath = relativePath.Replace('\\', '/'),
            FileName = uniqueName,
            OriginalFileName = originalFileName,
            ContentType = contentType ?? string.Empty,
            FileSize = fileInfo.Length,
            Sha256Hash = sha256
        };
    }

    /// <summary>
    /// Reads file bytes from a relative path under the storage root.
    /// </summary>
    /// <param name="relativePath">Relative path as returned by SaveFileAsync.</param>
    /// <returns>File bytes.</returns>
    public async Task<byte[]> ReadFileAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentException("Relative path is required", nameof(relativePath));
        var absolutePath = Path.Combine(_storageRoot, NormalizeSeparators(relativePath));
        return await File.ReadAllBytesAsync(absolutePath);
    }

    /// <summary>
    /// Deletes a file at a relative path if it exists.
    /// </summary>
    /// <param name="relativePath">Relative path as returned by SaveFileAsync.</param>
    /// <returns>True if deleted; false if the file did not exist.</returns>
    public Task<bool> DeleteFileAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentException("Relative path is required", nameof(relativePath));
        var absolutePath = Path.Combine(_storageRoot, NormalizeSeparators(relativePath));
        if (!File.Exists(absolutePath)) return Task.FromResult(false);
        File.Delete(absolutePath);
        return Task.FromResult(true);
    }

    // ID-based read/delete are handled by IFileHandler; this class focuses on disk I/O only.

    private static string BuildRelativePath(string category, DateTime timestampUtc, string fileName, string? outputSubfolder = null)
    {
        var safeCategory = string.IsNullOrWhiteSpace(category) ? "general" : category.Trim();
        var year = timestampUtc.ToString("yyyy", CultureInfo.InvariantCulture);
        var month = timestampUtc.ToString("MM", CultureInfo.InvariantCulture);
        var day = timestampUtc.ToString("dd", CultureInfo.InvariantCulture);

        if (!string.IsNullOrWhiteSpace(outputSubfolder))
        {
            var safeSubfolder = outputSubfolder.Trim().Replace('\\', '/');
            if (safeSubfolder.Contains("..", StringComparison.Ordinal))
            {
                throw new ArgumentException("Output subfolder must not contain '..'", nameof(outputSubfolder));
            }
            safeSubfolder = string.Join('/', safeSubfolder.Split('/', StringSplitOptions.RemoveEmptyEntries));
            return string.Join('/', safeSubfolder, year, month, day, fileName);
        }

        return string.Join('/', safeCategory, year, month, day, fileName);
    }

    private static string NormalizeSeparators(string path)
    {
        return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
    }

    private static string BytesToHex(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
