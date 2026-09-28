using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Represents the configuration for mapping the parent count query for a location.
/// </summary>
internal class LocationParentCountMapper : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Location Parent Count Mapper".
    /// </summary>
    /// <returns>
    /// A JSON string representing the mapping configuration for parent count queries, including rules and target parameters.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Name': 'locationparentcountmapper', 
                    'Type': 'Standard',
                    'Rules': [
                        { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                    ],
                    'Target' : { 
                        'Name': 'locationparentcount', 
                        'Parameters': [ 
                            {'Key': 'ParentLocationId', 'Value': '<:1:>'}
                        ] 
                    }
                    }";
    }
}

