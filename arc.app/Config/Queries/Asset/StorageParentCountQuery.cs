using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the configuration for the "Storage Parent Count Query".
/// </summary>
internal class StorageParentCountQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Storage Parent Count Query".
    /// </summary>
    /// <returns>
    /// A JSON string defining the configuration for counting parent storage records, 
    /// including parameter mappings, fields, and filtering conditions.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Query': 'storageparentcountquery', 'TableName': 'Storage', 'Type': 'Count',
                    'ParameterMapping': 'storageparentcountmapper',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'int'}
                    ],
                    'Where' : [
                        {'Field': 'ParentStorageId', 'Comparison': 'equals' }
                    ]
                }";
    }
}

