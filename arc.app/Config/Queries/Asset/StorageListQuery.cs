using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the query configuration for retrieving a list of storage entries.
/// </summary>
internal class StorageListQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Storage List" query.
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the query that retrieves storage entries.
    /// </returns>
    public string Get()
    {
        return @"{  
                'Query': 'storagelistquery', 'TableName': 'Storage', 'Type': 'Select', 'OrderBy' : 'Name',
                'Fields': [
                    {'Name': 'Id', 'Type': 'int'},
                    {'Name': 'StorageName', 'Type': 'string'},
                    {'Name': 'Code', 'Type': 'string'},
                    {'Name': 'FullyQualifiedName', 'Type': 'string'},
                    {'Name': 'Enabled', 'Type': 'string'}
                ],
                'ListItems': 'StorageType',
                'Where' : [
                    {'Field': 'Name', 'Comparison': 'contains', orGroup: 'search' },
                    {'Field': 'Code', 'Comparison': 'contains', orGroup: 'search' },
                    {'Field': 'StorageTypeId', 'Comparison': 'oneof' }
                ],
                'Orderby': 'StorageName',
                'Descending': false
            }";
    }
}

