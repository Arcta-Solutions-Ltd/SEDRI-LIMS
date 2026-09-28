using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Biochemistry data section.
/// This data section provides the available fields for biochemistry test results including glucose and protein.
/// </summary>
internal class BiochemistryDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Biochemistry data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "BiochemistryDataSection",
                "Title": "@TesBioA@",
                "Fields": [
                    {
                        "Label": "@TesGlu@",
                        "Value": "glucose"
                    },
                    {
                        "Label": "@TesPro@",
                        "Value": "protein"
                    }
                ]
            }
            """;
    }
}
