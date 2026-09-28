using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the configuration for the "Location Supplier Count Query".
/// </summary>
internal class LocationSupplierCountQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Location Supplier Count Query".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for counting suppliers associated with a specific location, 
    /// including fields and filtering conditions.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Query': 'locationsuppliercountquery', 'TableName': 'Supplier', 'Type': 'Count',
                    'ParameterMapping': 'locationsuppliercountmapper',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'int'}
                    ],
                    'Where' : [
                        {'Field': 'LocationId', 'Comparison': 'equals' }
                    ]
                }";
    }
}

