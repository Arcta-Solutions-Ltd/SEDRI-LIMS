using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the configuration for the "Location Parent Count Query".
/// </summary>
internal class LocationParentCountQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Location Parent Count Query".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for counting parent locations, including fields and conditions.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Query': 'locationparentcount', 'TableName': 'Location', 'Type': 'Count',
                    'ParameterMapping': 'locationparentcountmapper',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'int'}
                    ],
                    'Where' : [
                        {'Field': 'ParentLocationId', 'Comparison': 'equals' }
                    ]
                }";
    }
}

