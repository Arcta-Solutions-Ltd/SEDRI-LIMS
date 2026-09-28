using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides configuration for the 'managecultureattachmentsuievent' UI event.
/// </summary>
internal class ManageCultureAttachmentsUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    /// <returns>A JSON-formatted string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'managecultureattachmentsuievent',
            description: '@CulAtt@',
            type: 'form',
            action: 'managecultureattachmentsform'
        }";

        return newEvent;
    }
}
