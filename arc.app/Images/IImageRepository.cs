using arc.common.Models;
using arc.data.model.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Images;

public interface IImageRepository
{
    /// <summary>
    /// Gets all active images
    /// </summary>
    /// <returns>List of active images</returns>
    Task<IEnumerable<ImageDataModel>> GetImageListAsync();

    /// <summary>
    /// Gets an image by ID
    /// </summary>
    /// <param name="id">The image ID</param>
    /// <returns>The image model or null if not found</returns>
    Task<ImageDataModel> GetImageByIdAsync(int id);

    /// <summary>
    /// Adds a new image
    /// </summary>
    /// <param name="dataToSave">The image data as JSON string</param>
    /// <param name="token">The user token</param>
    /// <returns>The ID of the added image</returns>
    Task<int> AddAsync(string dataToSave);

    /// <summary>
    /// Updates an existing image
    /// </summary>
    /// <param name="dataToSave">The image data as JSON string</param>
    /// <returns>The ID of the updated image</returns>
    Task<int> EditAsync(string dataToSave);

    /// <summary>
    /// Soft deletes an image (sets IsActive to false)
    /// </summary>
    /// <param name="id">The image ID to delete</param>
    /// <returns>Number of rows affected</returns>
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Gets an image by name (for reports)
    /// </summary>
    /// <param name="name">The image name</param>
    /// <returns>The image data or null if not found</returns>
    Task<ImageDataModel> GetImageAsync(string name);
}
