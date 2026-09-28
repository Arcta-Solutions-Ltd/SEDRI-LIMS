using arc.app.Common;

namespace arc.app.Config.Mapper.Admission;

/// <summary>
/// Mapper for request attachments region on the request record view.
/// </summary>
internal class RequestAttachmentsViewMapper : IDefinition
{
    /// <summary>
    /// Retrieves the mapper configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            'Name': 'requestattachmentsviewmapper',
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
