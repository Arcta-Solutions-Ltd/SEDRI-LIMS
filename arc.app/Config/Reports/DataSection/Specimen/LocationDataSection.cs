using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Location data section.
/// This data section provides the available fields for location information.
/// </summary>
internal class LocationDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Location data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "LocationDataSection",
                "Title": "@GenLoc@",
                "Fields": [
                    {
                        "Label": "@GenLocA@",
                        "Value": "fullyqualifiedname"
                    }
                ]
            }
            """;
    }
}
