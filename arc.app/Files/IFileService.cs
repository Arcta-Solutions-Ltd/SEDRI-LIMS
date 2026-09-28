using arc.common.Models.Files;
using System;
using System.IO;
using System.Threading.Tasks;

namespace arc.app.Files
{
    /// <summary>Abstraction for file storage operations used by the application.</summary>
    public interface IFileService
    {
        /// <summary>Saves a file stream to disk under yyyy/MM/dd, optionally under a category.</summary>
        /// <param name="content">Readable stream for the file contents. Caller retains ownership.</param>
        /// <param name="originalFileName">Original filename (used to determine extension).</param>
        /// <param name="contentType">MIME type.</param>
        /// <param name="category">Top-level logical category (e.g., "image", "document").</param>
        /// <param name="timestampUtc">Timestamp to use for date folders; defaults to UtcNow.</param>
        /// <param name="outputSubfolder">Optional subfolder relative to storage root. When set, path is outputSubfolder/yyyy/MM/dd/filename.</param>
        /// <returns>Details about the saved file, including relative path.</returns>
        Task<FileUploadResult> SaveFileAsync(Stream content, string originalFileName, string contentType, string category = "image", DateTime? timestampUtc = null, string? outputSubfolder = null);

        /// <summary>Reads file bytes from a relative path (from storage root).</summary>
        /// <param name="relativePath">Relative path as returned by SaveFileAsync.</param>
        /// <returns>File bytes.</returns>
        Task<byte[]> ReadFileAsync(string relativePath);

        /// <summary>Deletes a file if it exists at the given relative path.</summary>
        /// <param name="relativePath">Relative path as returned by SaveFileAsync.</param>
        /// <returns>True if deleted; false if not found.</returns>
        Task<bool> DeleteFileAsync(string relativePath);
    }
}
