using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Pregnancy data section.
/// This data section provides the available fields for pregnancy test results.
/// </summary>
internal class PregnancyDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Pregnancy data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "PregnancyDataSection",
                "Title": "@TesPreA@",
                "Fields": [
                    {
                        "Label": "@TesPreRes@",
                        "Value": "pregnancyId"
                    }
                ]
            }
            """;
    }
}
