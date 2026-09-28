using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen;

/// <summary>
/// Mapper for manage culture attachments event. Maps Id (from record context) and FileAttachmentIds to the save payload.
/// </summary>
internal class ManageCultureAttachmentsMapper : IDefinition
{
    /// <summary>
    /// Retrieves the mapper configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            'Name': 'managecultureattachmentsmapper',
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
