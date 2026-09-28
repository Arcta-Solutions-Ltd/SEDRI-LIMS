using arc.app.Common;

namespace arc.app.Config.Queries.Asset;

/// <summary>
/// Represents the query configuration for retrieving a single storage entry from the storage list.
/// </summary>
internal class SingleStorageForStorageListQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Single Storage for Storage List" query.
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the query that retrieves a single storage entry.
    /// </returns>
    public string Get()
    {
        return @"{  
                'Query': 'storagelistquery', 'TableName': 'Storage', 'Type': 'Single',
                'Fields': [
                    {'Name': 'Id', 'Type': 'int'},
                    {'Name': 'StorageName', 'Type': 'string'},
                    {'Name': 'Code', 'Type': 'string'},
                    {'Name': 'FullyQualifiedName', 'Type': 'string'},
                    {'Name': 'Enabled', 'Type': 'string'}
                ],
                'ListItems': 'StorageType',
                'Where' : [
                    {'Field': 'Id', 'Comparison': '=' } 
                ]
            }";
    }
}

