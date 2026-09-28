using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a double column section format with two grid positions.
/// A section using this format places two of its data section's grids as separate tables rather than
/// gathering them into one, which is what a test with more than one fieldgrid needs.
/// </summary>
/// <remarks>
/// The description is plain text rather than a language catalogue token because no catalogue entry
/// exists for it; an untranslated token renders as an empty string. Formats created in the report
/// designer carry plain text descriptions in the same way.
/// </remarks>
internal class DoubleColumnWithTwoGridsConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the double column with two grids format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a two-column layout and two grid positions.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DoubleColumnWithTwoGrids",
                "Type": "DoubleFieldColumn",
                "Description": "Two Column With Two Grids Format",
                "Columns": [
                    {
                        "left": 100,
                        "width": 180,
                        "labelwidth": 100
                    },
                    {
                        "left": 300,
                        "width": 180,
                        "labelwidth": 100
                    }
                ],
                "Grids": [
                    {
                        "left": 100,
                        "width": "300|80"
                    },
                    {
                        "left": 100,
                        "width": "300|80"
                    }
                ]
            }
            """;
    }
}
