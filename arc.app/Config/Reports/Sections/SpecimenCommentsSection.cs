using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Specimen Comments Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class SpecimenCommentsSection : IDefinition
{
    /// <summary>
    /// Gets the Specimen Comments Section configuration.
    /// Grids includes the specimen comments entered during the test.
    /// </summary>
    /// <returns>JSON string containing the Specimen Comments Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "SpecimenCommentsSection",
                "Grids": [
                    {
                        "Head": [
                            "@GenComJ@"
                        ],
                        "Name": "SpecimenCommentsTable"
                    }
                ],
                "Fields": [
                ],
                "Format": "TableSingleColumn",
                "Type": "layout",
                "Dynamic": true,
                "DataSection": "SpecimenCommentsDataSection",
                "Description": "@GenComJ@"
            }
            """;
    }
}
