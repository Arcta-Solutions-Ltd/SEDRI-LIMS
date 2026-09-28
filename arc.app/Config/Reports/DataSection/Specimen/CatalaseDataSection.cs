using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Catalase data section.
/// This data section provides the available fields for catalase test results.
/// </summary>
internal class CatalaseDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Catalase data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CatalaseDataSection",
                "Title": "@TesCatA@",
                "Fields": [
                    {
                        "Label": "@TesCatRes@",
                        "Value": "catalaseresultId"
                    }
                ]
            }
            """;
    }
}
