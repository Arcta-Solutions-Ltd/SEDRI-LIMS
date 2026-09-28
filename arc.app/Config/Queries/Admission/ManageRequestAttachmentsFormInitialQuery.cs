using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Initial query for the manage request attachments form.
/// </summary>
internal class ManageRequestAttachmentsFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the manage request attachments form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ManageRequestAttachmentsFormInitialQuery',
            'TableName': 'Request',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'FileAttachmentIds', 'Type': 'requestfileattachmentids', 'KnownAs': 'FileAttachmentIds' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
