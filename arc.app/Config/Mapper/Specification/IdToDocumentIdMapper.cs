using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Maps the list item Id to DocumentId for the checkwhetherdocumentinspecification query.
/// </summary>
internal class IdToDocumentIdMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'idtodocumentidmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'checkwhetherdocumentinspecification', 
                            'Parameters': [ 
                                {'Key': 'DocumentId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
    }
}
