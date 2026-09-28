using arc.app.Common;
using arc.domain.Configuration.EventsConfig;
using arc.common;
using System.IO;
using System.Threading.Tasks;

namespace arc.app.Images.Events;

/// <summary>
/// Event to update an existing image
/// </summary>
public class UpdateImageEvent : IRun
{
    private readonly IImageRepository _imageRepository;

    public UpdateImageEvent(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
            return await _imageRepository.EditAsync(dataToSave);
    }
}
