using arc.app.Common;

namespace arc.app.Config.Mapper.Instruments;

/// <summary>
/// Maps aggregated file attachment ids to the instrument result record view attachments section (read-only gallery).
/// </summary>
internal class InstrumentResultAttachmentsViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
            'Name': 'instrumentresultattachmentsviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:1:>', Type: 'Mapping', Source: 'fileattachmentids', Value: 'fileattachmentids' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'instrumentresultattachments',
                        Title: '@InsUplFil@',
                        Fields: [
                            { Id: 'fileattachmentids', Label: '', Type: 'upload', Value: '<:1:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
