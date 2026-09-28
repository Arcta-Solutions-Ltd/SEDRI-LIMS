using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen;

/// <summary>
/// Mapper for specimen attachments region. Maps fileattachmentids to the attachments section display.
/// </summary>
internal class SpecimenAttachmentsViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
            'Name': 'specimenattachmentsviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:42:>', Type: 'Mapping', Source: 'fileattachmentids', Value: 'fileattachmentids' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'attachments',
                        Title: '@GenAtts@',
                        Fields: [
                            { Id: 'fileattachmentids', Label: '', Type: 'upload', Value: '<:42:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
