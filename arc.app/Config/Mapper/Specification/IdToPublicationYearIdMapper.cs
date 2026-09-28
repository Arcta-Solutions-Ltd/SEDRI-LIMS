using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Maps the list item Id to PublicationYearId for the checkwhetheryearinspecification query.
/// </summary>
internal class IdToPublicationYearIdMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'idtopublicationyearidmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'checkwhetheryearinspecification', 
                            'Parameters': [ 
                                {'Key': 'PublicationYearId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
    }
}
