using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Query for culture attachments display. Returns Id and FileAttachmentIds for the culture record view attachments region.
/// </summary>
internal class CultureAttachmentsForCultureViewQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'CultureAttachmentsForCultureView',
            'TableName': 'Culture',
            'Type': 'Single',
            'Translate': true,
            'ResultMapping': 'cultureattachmentsviewmapper',
            'Fields': [
                { 'Name': 'Id', 'KnownAs': 'id' },
                { 'Name': 'FileAttachmentIds', 'Type': 'culturefileattachmentids', 'KnownAs': 'fileattachmentids' }
            ],
            'Where': [
                {'Field': 'Id', 'Comparison': '=' }
            ],
            'Tags': 'SP'
        }";
    }
}
