using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides configuration for the 'managespecimenattachmentsuievent' UI event.
/// </summary>
internal class ManageSpecimenAttachmentsUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    /// <returns>A JSON-formatted string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'managespecimenattachmentsuievent',
            description: '@SpeAtt@',
            type: 'form',
            action: 'managespecimenattachmentsform'
        }";

        return newEvent;
    }
}
