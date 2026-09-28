using arc.app.Common;

namespace arc.app.Config.UIEvents.Request;

/// <summary>
/// Provides configuration for the managerequestattachmentsuievent UI event.
/// </summary>
internal class ManageRequestAttachmentsUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'managerequestattachmentsuievent',
            description: '@ReqAtt@',
            type: 'form',
            action: 'managerequestattachmentsform'
        }";
    }
}
