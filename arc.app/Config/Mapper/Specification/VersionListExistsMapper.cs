using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Maps add version form data to the versionlistitemexists query parameters.
/// ListId 134 = Version list.
/// </summary>
internal class VersionListExistsMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'versionlistexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'versionlistitemexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '134'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
    }
}
