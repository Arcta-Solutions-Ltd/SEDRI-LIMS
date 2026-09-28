using arc.app.Common;

namespace arc.app.Config.Mapper.Export;

/// <summary>
/// Mapper for export history attachments region. Maps fileattachmentid to the attachments section display.
/// </summary>
internal class ExportHistoryAttachmentsViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
            'Name': 'exporthistoryattachmentsviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:13:>', Type: 'Mapping', Source: 'fileattachmentid', Value: 'fileattachmentid' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'attachments',
                        Title: '@GenAtts@',
                        Fields: [
                            { Id: 'fileattachmentid', Label: '', Type: 'upload', Value: '<:13:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
