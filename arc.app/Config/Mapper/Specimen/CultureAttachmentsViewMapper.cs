using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen;

/// <summary>
/// Mapper for culture attachments region. Maps fileattachmentids to the attachments section display.
/// </summary>
internal class CultureAttachmentsViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
            'Name': 'cultureattachmentsviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:24:>', Type: 'Mapping', Source: 'fileattachmentids', Value: 'fileattachmentids' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'attachments',
                        Title: '@GenAtts@',
                        Fields: [
                            { Id: 'fileattachmentids', Label: '', Type: 'upload', Value: '<:24:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
