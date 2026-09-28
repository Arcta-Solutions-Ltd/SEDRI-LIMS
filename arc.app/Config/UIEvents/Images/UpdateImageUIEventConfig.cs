using arc.app.Common;

namespace arc.app.Config.UIEvents.Images;

/// <summary>
/// Configuration for the update image UI event
/// </summary>
internal class UpdateImageUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'name': 'updateimageuievent',
            'description': 'Update Image',
            'type': 'form',
            'action': 'updateimageform'
        }";
    }
}
