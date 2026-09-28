using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Initial query for the manage culture attachments form.
/// Returns Id and FileAttachmentIds (comma-separated) for the culture.
/// </summary>
internal class ManageCultureAttachmentsFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the manage culture attachments form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ManageCultureAttachmentsFormInitialQuery',
            'TableName': 'Culture',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'FileAttachmentIds', 'Type': 'culturefileattachmentids', 'KnownAs': 'FileAttachmentIds' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
