using arc.common.Models.Files;
using System.IO;
using System.Threading.Tasks;

namespace arc.app.Files
{
    /// <summary>
    /// Coordinates file operations by delegating to the physical file service and the data repository layer.
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Saves a file stream to disk (yyyy/MM/dd/category) and returns upload metadata.
        /// Database persistence of fileattachments may be performed by the handler implementation.
        /// </summary>
        /// <param name="content">Readable stream for file content.</param>
        /// <param name="originalFileName">Client-provided original filename (for extension).</param>
        /// <param name="contentType">MIME type.</param>
        /// <param name="category">Logical category (e.g., image, document).</param>
        /// <param name="outputSubfolder">Optional subfolder relative to storage root (e.g. exports/myschedule). When set, file is saved under outputSubfolder/yyyy/MM/dd/.</param>
        /// <returns>Upload metadata including relative path and content hash.</returns>
        Task<FileUploadResult> UploadAsync(Stream content, string originalFileName, string contentType, string category = "image", string? outputSubfolder = null);

        /// <summary>
        /// Reads a file by its identifier in fileattachments.
        /// </summary>
        /// <param name="id">Identifier in fileattachments.</param>
        /// <returns>File bytes, download name, and content type.</returns>
        Task<FileReadResult> ReadByIdAsync(int id);

        /// <summary>
        /// Deletes a file by its identifier in fileattachments.
        /// </summary>
        /// <param name="id">Identifier in fileattachments.</param>
        /// <returns>True if deleted; false if not found.</returns>
        Task<bool> DeleteByIdAsync(int id);
    }
}
