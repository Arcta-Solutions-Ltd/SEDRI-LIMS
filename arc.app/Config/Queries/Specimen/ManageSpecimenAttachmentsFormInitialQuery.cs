using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Initial query for the manage specimen attachments form.
/// Returns Id and FileAttachmentIds (comma-separated) for the specimen.
/// </summary>
internal class ManageSpecimenAttachmentsFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the manage specimen attachments form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ManageSpecimenAttachmentsFormInitialQuery',
            'TableName': 'Specimen',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'FileAttachmentIds', 'Type': 'specimenfileattachmentids', 'KnownAs': 'FileAttachmentIds' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
