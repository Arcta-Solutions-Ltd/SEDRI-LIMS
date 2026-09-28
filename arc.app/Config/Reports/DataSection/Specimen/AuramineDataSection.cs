using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Auramine data section.
/// This data section provides the available fields for auramine test results.
/// </summary>
internal class AuramineDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Auramine data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "AuramineDataSection",
                "Title": "@RepAur@",
                "Fields": [
                    {
                        "Label": "@TesAurRes@",
                        "Value": "AuramineId"
                    }
                ]
            }
            """;
    }
}
