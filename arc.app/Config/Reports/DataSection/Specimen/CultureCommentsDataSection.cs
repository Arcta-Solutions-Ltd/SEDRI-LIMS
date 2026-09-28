using arc.app.Common;

namespace arc.app.Config.Reports.DataSection.Specimen;

/// <summary>
/// Defines the configuration for the Culture Comments data section.
/// This data section provides a grid/table for displaying culture comments.
/// </summary>
internal class CultureCommentsDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Culture Comments data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with a grid for culture comments.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CultureCommentsDataSection",
                "Grids": [
                    {
                        "data": "CultureComments",
                        "name": "CommentsTable",
                        "description": "Culture Comments"
                    }
                ],
                "Title": "@GenComK@",
                "Fields": [
                ]
            }
            """;
    }
}
