using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Maps delete tag event message (Id) to taghaschildren query parameters (parentid).
/// Used when validating that a tag has no children before allowing delete.
/// </summary>
internal class TagHasChildrenMapper : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the tag has children mapper.
    /// </summary>
    public string Get()
    {
        return @"{
                    'Name': 'taghaschildrenmapper',
                    'Type': 'Standard',
                    'Rules': [
                        { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                    ],
                    'Target' : {
                        'Name': 'taghaschildren',
                        'Parameters': [
                            {'Key': 'parentid', 'Value': '<:1:>'}
                        ]
                    }
                }";
    }
}
