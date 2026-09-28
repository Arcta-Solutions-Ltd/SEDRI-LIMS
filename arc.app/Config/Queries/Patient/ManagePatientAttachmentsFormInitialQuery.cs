using arc.app.Common;

namespace arc.app.Config.Queries.Patient;

/// <summary>
/// Initial query for the manage patient attachments form.
/// Returns Id and FileAttachmentIds (comma-separated) for the patient.
/// </summary>
internal class ManagePatientAttachmentsFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the manage patient attachments form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ManagePatientAttachmentsFormInitialQuery',
            'TableName': 'Patient',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'FileAttachmentIds', 'Type': 'patientfileattachmentids', 'KnownAs': 'FileAttachmentIds' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
