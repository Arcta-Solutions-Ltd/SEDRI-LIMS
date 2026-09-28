using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Wet Prep data section.
/// This data section provides the available fields for wet prep results and includes a grid for parasite data.
/// </summary>
internal class WetPrepDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Wet Prep data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields and grids.</returns>
    public string Get()
    {
        return """
            {
                "Name": "WetPrepDataSection",
                "Title": "@RepWet@",
                "Fields": [
                    {
                        "Label": "@TesWetWbc@",
                        "Value": "wbcwetprep"
                    },
                    {
                        "Label": "@TesRbcB@",
                        "Value": "rbcwetprep"
                    }
                ],
                "Grids": [
                    {
                        "name": "ParasiteGrid",
                        "description": "Parasite",
                        "data": "WetPrep"
                    }
                ]
            }
            """;
    }
}
