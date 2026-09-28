using arc.app.Common;

namespace arc.app.Config.Events.Patient;

/// <summary>
/// Configuration for the "Manage Patient Attachments" event.
/// </summary>
internal class ManagePatientAttachmentsEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'managepatientattachments',
            Description: '@PatAtt@',
            EventType: 'specialadddata',
            Topic: 'Patient',
            TableName: 'Patient',
            RequiresSpecimenWorkflow: false,
            Mapping: 'managepatientattachmentsmapper',
            ValidationRules: [
                { field: 'Id', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
