using arc.app.Common;
using arc.app.Images;
using arc.common.Data;
using arc.common.Models;
using arc.data.Common;
using arc.data.model.Image;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Images;

/// <summary>
/// Repository class for handling image-related operations.
/// </summary>
/// <remarks>
/// This class extends the <see cref="GeneralRepository"/> and implements <see cref="IImageRepository"/>
/// to provide functionality for adding, editing, querying, and deleting image records.
/// </remarks>
public class ImageRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
    : GeneralRepository(sqlQuery, logWriter, sqlCommand), IImageRepository
{
    /// <summary>
    /// Adds a new image record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The data to save, provided as a JSON string.</param>
    /// <param name="token">The user-specific token containing authentication and contextual information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added image record.
    /// </returns>
    public async Task<int> AddAsync(string dataToSave)
    {
        var imageData = JsonConvert.DeserializeObject<ImageDataModel>(dataToSave);
        _logWriter.LogInfo("Run add image command", "ImageRepository", "AddAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddImageCommand(), "Add Image", imageData);
    }

    /// <summary>
    /// Edits an existing image record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The updated image data, provided as a JSON string.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the edited image record.
    /// </returns>
    public async Task<int> EditAsync(string dataToSave)
    {
        var image = JsonConvert.DeserializeObject<ImageDataModel>(dataToSave);
        _logWriter.LogInfo("Run edit image command", "ImageRepository", "EditAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new UpdateImageCommand(), "Edit Image", image);
    }

    /// <summary>
    /// Deletes an image record asynchronously.
    /// </summary>
    /// <param name="id">The ID of the image to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of rows affected.
    /// </returns>
    public async Task<int> DeleteAsync(int id)
    {
        _logWriter.LogInfo("Run delete image command", "ImageRepository", "DeleteAsync");
        return await _sqlCommand.CarryOutCommandReturningIntegerAsync(new DeleteImageCommand(), "Delete Image", id.ToString());
    }

    /// <summary>
    /// Retrieves a list of active images asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an enumerable list of active images.
    /// </returns>
    public async Task<IEnumerable<ImageDataModel>> GetImageListAsync()
    {
        _logWriter.LogInfo("Run get image list query", "ImageRepository", "GetImageListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ImageListQuery(), "Get Image List Query", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves a single image by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the image to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the image data model or null if not found.
    /// </returns>
    public async Task<ImageDataModel> GetImageByIdAsync(int id)
    {
        _logWriter.LogInfo("Run get single image query", "ImageRepository", "GetImageByIdAsync");
        var queryFilters = new QueryFilterConfig().AddString("id", id.ToString());
        return await _sqlQuery.QueryReturningTypeAsync(new SingleImageQuery(), "Get Single Image Query", queryFilters);
    }

    /// <summary>
    /// Retrieves an image by name asynchronously (for reports).
    /// </summary>
    /// <param name="name">The name of the image to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the image data model or null if not found.
    /// </returns>
    public async Task<ImageDataModel> GetImageAsync(string name)
    {
        _logWriter.LogInfo("Run get image by name query", "ImageRepository", "GetImageAsync");
        var queryFilters = new QueryFilterConfig().AddString("name", name);
        return await _sqlQuery.QueryReturningTypeAsync(new ImageByNameQuery(), "Get Image By Name Query", queryFilters);
    }
}
