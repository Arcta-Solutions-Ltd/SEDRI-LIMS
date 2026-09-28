using arc.app.Common;

namespace arc.app.Config.UIEvents.Admission;

/// <summary>
/// Provides configuration for the manageadmissionattachmentsuievent UI event.
/// </summary>
internal class ManageAdmissionAttachmentsUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'manageadmissionattachmentsuievent',
            description: '@AdmAtt@',
            type: 'form',
            action: 'manageadmissionattachmentsform'
        }";
    }
}
