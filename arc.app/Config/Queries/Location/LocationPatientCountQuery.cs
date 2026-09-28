using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the configuration for the "Location Patient Count Query".
/// </summary>
internal class LocationPatientCountQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Location Patient Count Query".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for a special query related to patient counts in locations.
    /// </returns>
    public string Get()
    {
        return @"{  
                        'Query': 'locationpatientcount', 'TableName': 'Patient', 'Type': 'Special'
            }";
    }
}

