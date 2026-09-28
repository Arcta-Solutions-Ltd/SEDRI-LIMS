using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the JEV (Japanese Encephalitis Virus) Serology data section.
/// This data section provides the available fields for JEV serology test results.
/// </summary>
internal class JevSerologyDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the JEV Serology data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "JevSerologyDataSection",
                "Title": "@TesJevA@",
                "Fields": [
                    {
                        "Label": "@TesJevRes@",
                        "Value": "jevserologyResultId"
                    }
                ]
            }
            """;
    }
}
