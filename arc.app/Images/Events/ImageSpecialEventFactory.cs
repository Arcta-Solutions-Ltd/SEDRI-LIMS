using arc.app.Common;
using arc.common.Models;

namespace arc.app.Images.Events;

/// <summary>
/// Small feature factory for image events, delegated to by SpecialEventFactory.
/// </summary>
public class ImageSpecialEventFactory : ISpecialEventFactory
{
    private readonly IImageRepository _imageRepository;

    public ImageSpecialEventFactory(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public IRun GetEvent(string eventName, TokenInfoModel token)
    {
        return eventName.ToLower() switch
        {
            "uploadimageevent" => new UploadImageEvent(_imageRepository),
            "updateimageevent" => new UpdateImageEvent(_imageRepository),
            "deleteimage" => new DeleteImageEvent(_imageRepository),
            _ => null,
        };
    }
}
