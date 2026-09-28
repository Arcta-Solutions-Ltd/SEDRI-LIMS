using arc.app.Common;

namespace arc.app.Config.Mapper.Admission;

/// <summary>
/// Mapper for admission attachments region on the admission record view.
/// Aggregates admission and child request attachment ids for display.
/// </summary>
internal class AdmissionAttachmentsViewMapper : IDefinition
{
    /// <summary>
    /// Retrieves the mapper configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            'Name': 'admissionattachmentsviewmapper',
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
