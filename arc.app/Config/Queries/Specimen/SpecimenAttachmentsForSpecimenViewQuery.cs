using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Query for specimen attachments display. Returns Id and FileAttachmentIds for the specimen record view attachments region.
/// </summary>
internal class SpecimenAttachmentsForSpecimenViewQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'SpecimenAttachmentsForSpecimenView',
            'TableName': 'Specimen',
            'Type': 'Single',
            'Translate': true,
            'ResultMapping': 'specimenattachmentsviewmapper',
            'Fields': [
                { 'Name': 'Id', 'KnownAs': 'id' },
                { 'Name': 'FileAttachmentIds', 'Type': 'specimenfileattachmentids', 'KnownAs': 'fileattachmentids' }
            ],
            'Where': [
                {'Field': 'Id', 'Comparison': '=' }
            ],
            'Tags': 'SP'
        }";
    }
}
