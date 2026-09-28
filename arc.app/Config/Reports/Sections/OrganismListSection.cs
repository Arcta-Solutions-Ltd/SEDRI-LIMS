using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Organism List Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, format, data section property
/// and grids.
/// </summary>
internal class OrganismListSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Organism List Section.
    /// Grids include an organism list table with antibiotic, sensitivity and special considerations.
    /// </summary>
    /// <returns>JSON string containing Organism List Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "OrganismListSection",
                "Description": "@GenOrgD@",
                "Format": "TableOne",
                "Type": "layout",
                "DataSection": "OrganismListDataSection",
                "Fields": [
                ],
                "Grids": [
                    {
                        "Name": "OrganismListTable",
                        "Head": [
                            "@RepAnt@",
                            "@RepSen@",
                            "@BreSpeB@",
                            "@GenMet@"
                        ]
                    }
                ]
            }
            """;
    }
}
