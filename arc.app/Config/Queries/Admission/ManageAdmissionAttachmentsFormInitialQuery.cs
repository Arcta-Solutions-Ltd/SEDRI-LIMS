using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Initial query for the manage admission attachments form.
/// </summary>
internal class ManageAdmissionAttachmentsFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the manage admission attachments form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ManageAdmissionAttachmentsFormInitialQuery',
            'TableName': 'Admission',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'FileAttachmentIds', 'Type': 'admissionfileattachmentids', 'KnownAs': 'FileAttachmentIds' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
