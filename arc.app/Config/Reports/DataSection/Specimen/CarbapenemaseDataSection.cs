using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Carbapenemase data section.
/// This data section provides the available fields for carbapenemase test results.
/// </summary>
internal class CarbapenemaseDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Carbapenemase data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CarbapenemaseDataSection",
                "Title": "@TesCarC@",
                "Fields": [
                    {
                        "Label": "@TesCarRes@",
                        "Value": "carbapenemaseresultId"
                    }
                ]
            }
            """;
    }
}
