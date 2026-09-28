using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides configuration for the 'managepatientattachmentsuievent' UI event.
/// </summary>
internal class ManagePatientAttachmentsUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    /// <returns>A JSON-formatted string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'managepatientattachmentsuievent',
            description: '@PatAtt@',
            type: 'form',
            action: 'managepatientattachmentsform'
        }";

        return newEvent;
    }
}
