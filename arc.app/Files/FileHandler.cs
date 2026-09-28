using System;
using System.IO;
using System.Threading.Tasks;
using arc.app.Common;
using arc.common.Models.Files;
using arc.data.model.File;

namespace arc.app.Files;

/// <summary>High-level coordinator for file operations (disk + DB).</summary>
public class FileHandler : IFileHandler
{
    private readonly IFileService _fileService;
    private readonly IGeneralRepository _generalRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileHandler"/> class.
    /// </summary>
    /// <param name="fileService">Service for physical file operations.</param>
    /// <param name="generalRepository">Repository for DB reads/writes (fileattachments).</param>
    public FileHandler(IFileService fileService, IGeneralRepository generalRepository)
    {
        _fileService = fileService;
        _generalRepository = generalRepository;
    }

    /// <inheritdoc />
    public async Task<FileUploadResult> UploadAsync(Stream content, string originalFileName, string contentType, string category = "image", string? outputSubfolder = null)
    {
        if (content == null) throw new ArgumentNullException(nameof(content));
        var saved = await _fileService.SaveFileAsync(content, originalFileName, contentType, category, null, outputSubfolder);

        var row = new FileAttachmentDataModel
        {
            Filepath = saved.RelativePath,
            Filename = saved.FileName,
            Originalfilename = saved.OriginalFileName,
            Contenttype = saved.ContentType,
            Filesize = (int)Math.Min(saved.FileSize, int.MaxValue),
            Sha256Hash = saved.Sha256Hash,
            IsActive = true
        };

        try
        {
            saved.Id = await _generalRepository.AddAsync(row, "fileattachments");
            return saved;
        }
        catch
        {
            try { await _fileService.DeleteFileAsync(saved.RelativePath); } catch { }
            throw;
        }
    }

    public async Task<FileReadResult> ReadByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Invalid id", nameof(id));

        var row = await _generalRepository.GetByIdAsync<FileAttachmentDataModel>("fileattachments", id);
        if (row == null || string.IsNullOrWhiteSpace(row.Filepath)) throw new FileNotFoundException("File not found for id.");

        var bytes = await _fileService.ReadFileAsync(row.Filepath).ConfigureAwait(false);
        var downloadName = string.IsNullOrWhiteSpace(row.Originalfilename) ? row.Filename : row.Originalfilename;
        var type = string.IsNullOrWhiteSpace(row.Contenttype) ? "application/octet-stream" : row.Contenttype;

        return new FileReadResult
        {
            Bytes = bytes,
            DownloadName = downloadName,
            ContentType = type
        };
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Invalid id", nameof(id));

        var row = await _generalRepository.GetByIdAsync<FileAttachmentDataModel>("fileattachments", id);
        if (row == null || string.IsNullOrWhiteSpace(row.Filepath)) return false;
        var deleted = await _fileService.DeleteFileAsync(row.Filepath).ConfigureAwait(false);
        if (!deleted) return false;
        row.IsActive = false;
        await _generalRepository.UpdateAsync(row, "fileattachments", nameof(FileAttachmentDataModel.Id));
        return true;
    }
}
