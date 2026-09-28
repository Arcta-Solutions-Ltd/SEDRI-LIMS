using arc.app.Common;
using arc.domain.Configuration.EventsConfig;
using arc.common;
using System.Threading.Tasks;

namespace arc.app.Images.Events;

/// <summary>
/// Legacy event to upload a new image - DEPRECATED
/// Replaced by new file upload system via FileController
/// </summary>
public class UploadImageEvent : IRun
{
    private readonly IImageRepository _imageRepository;

    /// <summary>
    /// Constructor for legacy upload image event
    /// </summary>
    /// <param name="imageRepository">Image repository dependency</param>
    public UploadImageEvent(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    /// <summary>
    /// Legacy method to upload image data - DEPRECATED
    /// Use FileController.UploadAsync instead
    /// </summary>
    /// <param name="dataToSave">Image data to save</param>
    /// <param name="id">Event ID</param>
    /// <param name="command">Event command</param>
    /// <param name="eventData">Event configuration data</param>
    /// <returns>Image ID</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        return await _imageRepository.AddAsync(dataToSave);
    }
}
