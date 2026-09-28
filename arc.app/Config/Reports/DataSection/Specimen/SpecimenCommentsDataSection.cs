using arc.app.Common;

namespace arc.app.Config.Reports.DataSection.Specimen;

/// <summary>
/// Defines the configuration for the Specimen Comments data section.
/// This data section provides a grid/table for displaying specimen comments.
/// </summary>
internal class SpecimenCommentsDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Specimen Comments data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with a grid for specimen comments.</returns>
    public string Get()
    {
        return """
            {
                "Name": "SpecimenCommentsDataSection",
                "Grids": [
                    {
                        "data": "SpecimenComments",
                        "name": "SpecimenCommentsTable",
                        "description": "Specimen Comments"
                    }
                ],
                "Title": "@GenComJ@",
                "Fields": [
                ]
            }
            """;
    }
}
