using arc.app.Images;
using arc.data.model.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// Controller for image operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImageController : ControllerBase
{
    private readonly IImageRepository _imageRepository;
    private readonly ILogger<ImageController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageController"/> class.
    /// </summary>
    /// <param name="imageRepository">Repository for image operations.</param>
    /// <param name="logger">Logger for internal error logging.</param>
    public ImageController(IImageRepository imageRepository, ILogger<ImageController> logger)
    {
        _imageRepository = imageRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets an image file by ID
    /// </summary>
    /// <param name="id">The image ID</param>
    /// <returns>The image file</returns>
    //[HttpGet("{id}/file")]
    //public async Task<IActionResult> GetImageFile(int id)
    //{
    //    try
    //    {
    //        var image = await _imageRepository.GetImageByIdAsync(id);
    //        if (image == null)
    //        {
    //            return NotFound();
    //        }

    //        var imageData = await _imageRepository.GetImageDataAsync(id);
    //        if (imageData == null)
    //        {
    //            return NotFound();
    //        }

    //        return File(imageData, image.ContentType, image.OriginalFileName);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, new { error = ex.Message });
    //    }
    //}

    /// <summary>
    /// Gets image metadata by ID
    /// </summary>
    /// <param name="id">The image ID</param>
    /// <returns>The image metadata</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(int id)
    {
        try
        {
            var image = await _imageRepository.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                id = image.Id,
                //filename = image.FileName,
                //originalfilename = image.OriginalFileName,
                //contenttype = image.ContentType,
                //filesize = image.FileSize,
                //width = image.Width,
                //height = image.Height,
                //description = image.Description,
                //createddate = image.CreatedDate,
                //updateddate = image.UpdatedDate
            });
        }
        catch (Exception ex)
        {
            // WAPT-006: Log detailed error internally, return generic message to client
            _logger.LogError(ex, "Error in {Operation}", nameof(GetImage));
            return StatusCode(500, new { error = "A problem happened while handling your request." });
        }
    }

    /// <summary>
    /// Gets all active images
    /// </summary>
    /// <returns>List of active images</returns>
    [HttpGet]
    public async Task<IActionResult> GetImages()
    {
        try
        {
            var images = await _imageRepository.GetImageListAsync();
            var result = images.Select(img => new
            {
                id = img.Id,
                name = img.Name,
                description = img.Description,
                fileAttachmentId = img.FileAttachmentId
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            // WAPT-006: Log detailed error internally, return generic message to client
            _logger.LogError(ex, "Error in {Operation}", nameof(GetImages));
            return StatusCode(500, new { error = "A problem happened while handling your request." });
        }
    }
}
