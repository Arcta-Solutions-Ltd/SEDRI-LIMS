using arc.app.Common;
using arc.domain.Configuration.EventsConfig;
using arc.common;
using System.Threading.Tasks;

namespace arc.app.Images.Events;

/// <summary>
/// Event to delete an image
/// </summary>
public class DeleteImageEvent : IRun
{
    private readonly IImageRepository _imageRepository;

    public DeleteImageEvent(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        return await _imageRepository.DeleteAsync(int.Parse(id));
    }
}
