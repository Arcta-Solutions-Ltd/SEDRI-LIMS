using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Manage Specimen Attachments" event.
/// </summary>
internal class ManageSpecimenAttachmentsEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'managespecimenattachments',
            Description: '@SpeAtt@',
            EventType: 'specialadddata',
            Topic: 'Specimen',
            TableName: 'Specimen',
            Mapping: 'managespecimenattachmentsmapper',
            ValidationRules: [
                { field: 'Id', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
