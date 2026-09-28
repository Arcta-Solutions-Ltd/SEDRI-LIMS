using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the configuration for the "Edit Storage Query".
/// </summary>
internal class EditStorageQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Edit Storage Query".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration details for the query, including fields and conditions.
    /// </returns>
    public string Get()
    {
        return @"{  
                'Query': 'editstoragequery', 'TableName': 'Storage', 'Type': 'Single',
                'Fields': [
                    {'Name': 'Id', 'Type': 'int'},
                    {'Name': 'StorageName', 'Type': 'string'},
                    {'Name': 'Code', 'Type': 'string'},
                    {'Name': 'Description', 'Type': 'string'},
                    {'Name': 'StorageTypeId', 'Type': 'int'},
                    {'Name': 'Temperature', 'Type': 'decimal'},
                    {'Name': 'ParentStorageId', 'Type': 'int'},
                    {'Name': 'Enabled', 'Type': 'string'}
                ],
                'Where' : [
                    {'Field': 'Id', 'Comparison': '=' } 
                ]
            }";
    }
}


