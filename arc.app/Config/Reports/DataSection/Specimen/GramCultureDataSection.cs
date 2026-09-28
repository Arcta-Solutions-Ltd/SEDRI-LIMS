using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Gram Culture data section.
/// This data section provides the available fields for gram culture results and includes a grid for organism data.
/// </summary>
internal class GramCultureDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Gram Culture data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields and grids.</returns>
    public string Get()
    {
        return """
            {
                "Name": "GramCultureDataSection",
                "Title": "@RepGra@",
                "Fields": [
                    {
                        "Label": "@TesEpi@",
                        "Value": "gcEpicells"
                    }
                ],
                "Grids": [
                    {
                        "name": "GramCultureTable",
                        "description": "Organism",
                        "data": "gcorganismgrid"
                    }
                ]
            }
            """;
    }
}
