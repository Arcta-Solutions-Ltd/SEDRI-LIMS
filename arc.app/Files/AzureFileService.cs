using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using arc.common.Models.Files;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace arc.app.Files;

/// <summary>Persists files to Azure Blob Storage under yyyy/MM/dd and serves by fileattachments id.</summary>
public class AzureFileService : IFileService
{
    private readonly BlobContainerClient _containerClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureFileService"/> class.
    /// </summary>
    /// <param name="blobServiceUri">Azure Blob Service URI.</param>
    /// <param name="containerName">Container name where blobs should be stored.</param>
    /// <param name="connectionString">Optional connection string for authentication. If null, uses DefaultAzureCredential.</param>
    public AzureFileService(string blobServiceUri, string containerName, string connectionString = null)
    {
        if (string.IsNullOrWhiteSpace(blobServiceUri))
            throw new ArgumentException("Blob service URI is required", nameof(blobServiceUri));
        if (string.IsNullOrWhiteSpace(containerName))
            throw new ArgumentException("Container name is required", nameof(containerName));

        BlobServiceClient blobServiceClient;

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            // Use connection string authentication (local dev if not signed in or explicit credentials)
            blobServiceClient = new BlobServiceClient(connectionString);
        }
        else
        {
            // Use DefaultAzureCredential (managed identity, Azure CLI, VS, etc.)
            blobServiceClient = new BlobServiceClient(new Uri(blobServiceUri), new DefaultAzureCredential());
        }

        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    /// <summary>
    /// Saves a file stream to Azure Blob Storage using a date-based folder structure and returns metadata.
    /// </summary>
    /// <param name="content">Readable stream for the file contents.</param>
    /// <param name="originalFileName">Original filename to preserve extension.</param>
    /// <param name="contentType">MIME content type.</param>
    /// <param name="category">Top-level logical category under storage root.</param>
    /// <param name="timestampUtc">Timestamp to use for date folder resolution.</param>
    /// <param name="outputSubfolder">Optional subfolder relative to storage root. When set, path is outputSubfolder/yyyy/MM/dd/filename.</param>
    /// <returns>Upload metadata including relative path and content hash.</returns>
    public async Task<FileUploadResult> SaveFileAsync(Stream content, string originalFileName, string contentType, string category = "image", DateTime? timestampUtc = null, string? outputSubfolder = null)
    {
        if (content == null) throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrWhiteSpace(originalFileName)) throw new ArgumentException("Original file name is required", nameof(originalFileName));

        var now = timestampUtc ?? DateTime.UtcNow;

        var extension = Path.GetExtension(originalFileName);
        var uniqueName = Guid.NewGuid().ToString("N") + extension;

        var relativePath = BuildRelativePath(category, now, uniqueName, outputSubfolder);
        var blobClient = _containerClient.GetBlobClient(relativePath);

        // Compute SHA-256 before upload
        string sha256;
        long fileSize;
        using (var sha = SHA256.Create())
        {
            var hash = await sha.ComputeHashAsync(content);
            sha256 = BytesToHex(hash);
            fileSize = content.Length;
        }

        // Reset stream position for upload
        content.Position = 0;

        // Upload to Azure Blob Storage
        await blobClient.UploadAsync(content, overwrite: false);

        // Set content type metadata
        if (!string.IsNullOrWhiteSpace(contentType))
        {
            await blobClient.SetHttpHeadersAsync(new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = contentType
            });
        }

        return new FileUploadResult
        {
            RelativePath = relativePath,
            FileName = uniqueName,
            OriginalFileName = originalFileName,
            ContentType = contentType ?? string.Empty,
            FileSize = fileSize,
            Sha256Hash = sha256
        };
    }

    /// <summary>
    /// Reads file bytes from a relative path in Azure Blob Storage.
    /// </summary>
    /// <param name="relativePath">Relative path as returned by SaveFileAsync.</param>
    /// <returns>File bytes.</returns>
    public async Task<byte[]> ReadFileAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("Relative path is required", nameof(relativePath));

        var blobClient = _containerClient.GetBlobClient(relativePath);

        using var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Deletes a file at a relative path if it exists in Azure Blob Storage.
    /// </summary>
    /// <param name="relativePath">Relative path as returned by SaveFileAsync.</param>
    /// <returns>True if deleted; false if the blob did not exist.</returns>
    public async Task<bool> DeleteFileAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("Relative path is required", nameof(relativePath));

        var blobClient = _containerClient.GetBlobClient(relativePath);
        var response = await blobClient.DeleteIfExistsAsync();
        return response.Value;
    }

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
