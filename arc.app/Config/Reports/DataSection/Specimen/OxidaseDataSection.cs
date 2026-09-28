using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Oxidase data section.
/// This data section provides the available fields for oxidase test results.
/// </summary>
internal class OxidaseDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Oxidase data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "OxidaseDataSection",
                "Title": "@TesOxiA@",
                "Fields": [
                    {
                        "Label": "@TesOxiRes@",
                        "Value": "oxidaseresultId"
                    }
                ]
            }
            """;
    }
}
