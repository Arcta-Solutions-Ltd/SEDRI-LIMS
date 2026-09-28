using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Gram Stain data section.
/// This data section provides the available fields for gram stain results and includes a grid for organism data.
/// </summary>
internal class GramStainDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Gram Stain data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields and grids.</returns>
    public string Get()
    {
        return """
            {
                "Name": "GramStainDataSection",
                "Title": "@RepGra@",
                "Fields": [
                    {
                        "Label": "@TesGraWbc@",
                        "Value": "wbc"
                    },
                    {
                        "Label": "@TesEpi@",
                        "Value": "Epicells"
                    }
                ],
                "Grids": [
                    {
                        "name": "OrganismGrid",
                        "description": "Organism",
                        "data": "gramstain"
                    }
                ]
            }
            """;
    }
}
