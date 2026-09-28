using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Provides the result mapping definition configuration for retrieving test list details for a specimen.
/// </summary>
internal class TestListForSpecimenResultMapper : IDefinition
{
    /// <summary>
    /// Gets the JSON-formatted result mapping definition.
    /// </summary>
    /// <returns>A string containing the JSON configuration for the result mapper.</returns>
    public string Get()
    {
        return @"{  
                    'Name': 'testlistforspecimenresultmapper', 'Type': 'Special'
                 }";
    }
}
