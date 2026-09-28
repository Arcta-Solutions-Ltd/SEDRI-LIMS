using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Culture Comments Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class CultureCommentsSection : IDefinition
{
    /// <summary>
    /// Gets the Culture Comments Section configuration.
    /// Grids includes the culture comments entered during the test.
    /// </summary>
    /// <returns>JSON string containing the Culture Comments Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CultureCommentsSection",
                "Grids": [
                    {
                        "Head": [
                            "@GenComK@"
                        ],
                        "Name": "CommentsTable"
                    }
                ],
                "Fields": [
                ],
                "Format": "TableSingleColumn",
                "Type": "layout",
                "Dynamic": true,
                "DataSection": "CultureCommentsDataSection",
                "Description": "@GenComK@"
            }
            """;
    }
}
