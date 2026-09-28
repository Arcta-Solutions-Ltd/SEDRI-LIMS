using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Manage Culture Attachments" event.
/// </summary>
internal class ManageCultureAttachmentsEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'managecultureattachments',
            Description: '@CulAtt@',
            EventType: 'specialadddata',
            Topic: 'Culture',
            TableName: 'Culture',
            Mapping: 'managecultureattachmentsmapper',
            ValidationRules: [
                { field: 'Id', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
