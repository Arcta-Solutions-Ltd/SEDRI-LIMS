using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen;

/// <summary>
/// Mapper for manage specimen attachments event. Maps Id (from record context) and FileAttachmentIds to the save payload.
/// </summary>
internal class ManageSpecimenAttachmentsMapper : IDefinition
{
    /// <summary>
    /// Retrieves the mapper configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            'Name': 'managespecimenattachmentsmapper',
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
