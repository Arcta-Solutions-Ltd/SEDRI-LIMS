using arc.app.Common;

namespace arc.app.Config.Queries.Patient;

/// <summary>
/// Query for patient attachments display. Returns aggregated patient, admission and request
/// attachment ids for the patient record view attachments region.
/// </summary>
internal class PatientAttachmentsForPatientViewQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'PatientAttachmentsForPatientView',
            'TableName': 'Patient',
            'Type': 'Single',
            'Translate': true,
            'ResultMapping': 'patientattachmentsviewmapper',
            'Fields': [
                { 'Name': 'Id', 'KnownAs': 'id' },
                { 'Name': 'FileAttachmentIds', 'Type': 'patientallfileattachmentids', 'KnownAs': 'fileattachmentids' }
            ],
            'Where': [
                {'Field': 'Id', 'Comparison': '=' }
            ],
            'Tags': 'SP'
        }";
    }
}
