using arc.app.Common;

namespace arc.app.Config.Events.Request;

/// <summary>
/// Configuration for the manage request attachments event.
/// </summary>
internal class ManageRequestAttachmentsEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'managerequestattachments',
            Description: '@ReqAtt@',
            EventType: 'specialadddata',
            Topic: 'Request',
            TableName: 'Request',
            RequiresSpecimenWorkflow: false,
            Mapping: 'managerequestattachmentsmapper',
            ValidationRules: [
                { field: 'Id', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
