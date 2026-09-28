using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Preculture Results data section.
/// This data section provides the available fields for preculture result information including specimen appearance.
/// </summary>
internal class PrecultureResultsDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Preculture Results data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "PrecultureResultsDataSection",
                "Title": "@RepPre@",
                "Fields": [
                    {
                        "Label": "@RepApp@",
                        "Value": "SpecimenAppearance"
                    }
                ]
            }
            """;
    }
}
