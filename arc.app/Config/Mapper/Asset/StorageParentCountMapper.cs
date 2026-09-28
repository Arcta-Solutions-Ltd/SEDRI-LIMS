using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Represents the configuration for mapping the parent count query for storage records.
/// </summary>
internal class StorageParentCountMapper : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Storage Parent Count Mapper".
    /// </summary>
    /// <returns>
    /// A JSON string representing the mapping configuration for parent count queries, 
    /// including rules and target parameters.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Name': 'storageparentcountmapper', 
                    'Type': 'Standard',
                    'Rules': [
                        { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                    ],
                    'Target' : { 
                        'Name': 'storageparentcountquery', 
                        'Parameters': [ 
                            {'Key': 'ParentStorageId', 'Value': '<:1:>'}
                        ] 
                    }
                }";
    }
}

