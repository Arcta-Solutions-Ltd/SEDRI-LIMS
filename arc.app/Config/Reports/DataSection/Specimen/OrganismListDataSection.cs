using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Organism List data section.
/// This data section provides a grid/table for displaying organism list data.
/// </summary>
internal class OrganismListDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Organism List data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with a grid for organism list data.</returns>
    public string Get()
    {
        return """
            {
                "Name": "OrganismListDataSection",
                "Title": "@GenOrgD@",
                "Fields": [
                ],
                "Grids": [
                    {
                        "name": "OrganismListTable",
                        "description": "Organism List",
                        "data": "OrganismList"
                    }
                ]
            }
            """;
    }
}
