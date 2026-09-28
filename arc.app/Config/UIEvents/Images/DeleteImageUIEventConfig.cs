using arc.app.Common;

namespace arc.app.Config.UIEvents.Images;

/// <summary>
/// Configuration for the delete image UI event
/// </summary>
internal class DeleteImageUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'name': 'deleteimageuievent',
            'description': 'Delete Image',
            'type': 'form',
            'action': 'deleteimageform',
        }";
    }
}
