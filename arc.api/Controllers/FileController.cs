using arc.app.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using arc.common.Models.Files;

namespace arc.api.Controllers;

/// <summary>
/// Provides API endpoints for uploading files and retrieving or deleting files by their fileattachments identifier.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class FileController : ControllerBase
{
    private readonly IFileHandler _fileHandler;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileController"/> class.
    /// </summary>
    /// <param name="fileHandler">Coordinator for disk and DB file operations.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="logger">Logger for internal error logging.</param>
    public FileController(IFileHandler fileHandler, IConfiguration configuration, ILogger<FileController> logger)
    {
        _fileHandler = fileHandler;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Uploads a file using multipart/form-data and returns metadata about the saved file.
    /// </summary>
    /// <returns>An action result containing the upload metadata.</returns>
    [HttpPost("upload")]
    [RequestSizeLimit(long.MaxValue)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<FileUploadResult>> UploadAsync()
    {
        var form = HttpContext.Request.Form;
        var file = form.Files["file"];
        var category = string.IsNullOrWhiteSpace(form["category"]) ? "image" : form["category"].ToString();

        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }

        try
        {
            // Server-side validation for content types/extensions
            var nameLower = (file.FileName ?? string.Empty).ToLowerInvariant();
            var blockedExt = _configuration.GetSection("Files:BlockedExtensions").Get<string[]>() ?? new[] { ".exe", ".msi", ".bat", ".cmd", ".ps1", ".sh", ".dll", ".com", ".scr" };
            foreach (var ext in blockedExt)
            {
                if (!string.IsNullOrWhiteSpace(ext) && nameLower.EndsWith(ext.ToLowerInvariant()))
                    return BadRequest("File type not permitted.");
            }
            var allowed = _configuration.GetSection("Files:AllowedContentTypes").Get<string[]>() ?? Array.Empty<string>();
            if (allowed.Length > 0)
            {
                var ct = (file.ContentType ?? string.Empty).ToLowerInvariant();
                var ok = false;
                foreach (var t in allowed)
                {
                    if (string.IsNullOrWhiteSpace(t)) continue;
                    if (t.StartsWith('.')) { if (nameLower.EndsWith(t.ToLowerInvariant())) { ok = true; break; } }
                    else { if (ct == t.ToLowerInvariant()) { ok = true; break; } }
                }
                if (!ok) return BadRequest("File type not permitted.");
            }

            using var stream = file.OpenReadStream();
            var result = await _fileHandler.UploadAsync(stream, file.FileName, file.ContentType, category);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // WAPT-006: Log detailed error internally, return generic message to client
            _logger.LogError(ex, "Error in {Operation}", nameof(UploadAsync));
            return StatusCode(500, new { error = "A problem happened while handling your request." });
        }
    }

    /// <summary>
    /// Returns lightweight metadata for a file by its identifier.
    /// </summary>
    /// <param name="id">The fileattachments identifier.</param>
    /// <returns>JSON containing id, originalFileName, fileSize, and contentType.</returns>
    [HttpGet("{id}/metadata")]
    public async Task<IActionResult> GetMetadataByIdAsync(int id)
    {
        if (id <= 0) return BadRequest("Invalid id");
        try
        {
            // Reuse existing read to derive metadata without new repository plumbing
            var file = await _fileHandler.ReadByIdAsync(id).ConfigureAwait(false);
            var size = file?.Bytes?.LongLength ?? 0L;
            var fileName = file?.DownloadName ?? string.Empty;
            _logger.LogDebug("GetMetadataById succeeded for id={Id}, filename={FileName}", id, fileName);
            return Ok(new
            {
                id,
                originalFileName = fileName,
                fileSize = size,
                contentType = file?.ContentType ?? "application/octet-stream"
            });
        }
        catch (Exception ex)
        {
            // WAPT-006: Log detailed error internally, return generic message to client
            _logger.LogError(ex, "Error in {Operation}", nameof(GetMetadataByIdAsync));
            return StatusCode(500, new { error = "A problem happened while handling your request." });
        }
    }

    /// <summary>
    /// Downloads a file by its identifier from the <c>fileattachments</c> table.
    /// </summary>
    /// <param name="id">The fileattachments identifier.</param>
    /// <returns>The file content to download.</returns>
    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadByIdAsync(int id)
    {
        if (id <= 0) return BadRequest("Invalid id");

        try
        {
            var file = await _fileHandler.ReadByIdAsync(id);
            _logger.LogDebug("DownloadById succeeded for id={Id}, filename={FileName}", id, file?.DownloadName ?? string.Empty);
            return File(file.Bytes, file.ContentType, file.DownloadName);
        }
        catch (Exception ex)
        {
            // WAPT-006: Log detailed error internally, return generic message to client
            _logger.LogError(ex, "Error in {Operation}", nameof(DownloadByIdAsync));
            return StatusCode(500, new { error = "A problem happened while handling your request." });
        }
    }

    /// <summary>
    /// Deletes a file by its identifier from the <c>fileattachments</c> table.
    /// </summary>
    /// <param name="id">The fileattachments identifier.</param>
    /// <returns>No content when deletion succeeds; NotFound if not present.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsync(int id)
    {
        if (id <= 0) return BadRequest("Invalid id");

        try
        {
            var deleted = await _fileHandler.DeleteByIdAsync(id).ConfigureAwait(false);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            // WAPT-006: Log detailed error internally, return generic message to client
            _logger.LogError(ex, "Error in {Operation}", nameof(DeleteByIdAsync));
            return StatusCode(500, new { error = "A problem happened while handling your request." });
        }
    }
}
