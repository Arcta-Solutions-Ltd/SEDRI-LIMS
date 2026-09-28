using arc.app.Common;

namespace arc.app.Config.Mapper.Admission;

/// <summary>
/// Mapper for manage admission attachments event.
/// </summary>
internal class ManageAdmissionAttachmentsMapper : IDefinition
{
    /// <summary>
    /// Retrieves the mapper configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            'Name': 'manageadmissionattachmentsmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                { Key: '<:2:>', Type: 'Mapping', Source: 'FileAttachmentIds', Value: 'FileAttachmentIds' }
            ],
            'Target': {
                'Id': '<:1:>',
                'FileAttachmentIds': '<:2:>'
            }
        }";
    }
}
