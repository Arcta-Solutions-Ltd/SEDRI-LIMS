using arc.app.Common;

namespace arc.app.Config.Events.Admission;

/// <summary>
/// Configuration for the manage admission attachments event.
/// </summary>
internal class ManageAdmissionAttachmentsEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'manageadmissionattachments',
            Description: '@AdmAtt@',
            EventType: 'specialadddata',
            Topic: 'Admission',
            TableName: 'Admission',
            RequiresSpecimenWorkflow: false,
            Mapping: 'manageadmissionattachmentsmapper',
            ValidationRules: [
                { field: 'Id', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
