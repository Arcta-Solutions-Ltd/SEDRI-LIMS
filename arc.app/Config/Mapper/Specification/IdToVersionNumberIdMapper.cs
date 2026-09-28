using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Maps the list item Id to VersionNumberId for the checkwhetherversioninspecification query.
/// </summary>
internal class IdToVersionNumberIdMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'idtoversionnumberidmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'checkwhetherversioninspecification', 
                            'Parameters': [ 
                                {'Key': 'VersionNumberId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
    }
}
