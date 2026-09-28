using arc.app.Common;

namespace arc.app.Config.UIEvents.Images;

/// <summary>
/// Configuration for the upload image UI event
/// </summary>
internal class UploadImageUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'name': 'uploadimageuievent',
            'description': 'Upload Image',
            'type': 'form',
            'action': 'uploadimageform'
        }";
    }
}
