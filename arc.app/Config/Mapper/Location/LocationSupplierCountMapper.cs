using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Represents the configuration for mapping the supplier count query for a location.
/// </summary>
internal class LocationSupplierCountMapper : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Location Supplier Count Mapper".
    /// </summary>
    /// <returns>
    /// A JSON string representing the mapping configuration for supplier count queries, including rules and target parameters.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Name': 'locationsuppliercountmapper', 
                    'Type': 'Standard',
                    'Rules': [
                        { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                    ],
                    'Target' : { 
                        'Name': 'locationsuppliercountquery', 
                        'Parameters': [ 
                            {'Key': 'LocationId', 'Value': '<:1:>'}
                        ] 
                    }
                }";
    }
}

