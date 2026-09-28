using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a double column section format with two grid layout.
/// This format provides a layout with two columns for fields and includes a grid/table for displaying tabular data.
/// </summary>
internal class DoubleColumnWithGridTwoConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the double column with grid two format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a two-column layout and one grid.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DoubleColumnWithGridTwo",
                "Type": "DoubleFieldColumn",
                "Description": "@RepTwoD@",
                "Heading": [
                    {
                        "Line": 1,
                        "Left": 20,
                        "Text": "@RepGra@",
                        "FontSize": 12,
                        "Bold": true
                    }
                ],
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
                        "width": "120|100|100|100"
                    }
                ]
            }
            """;
    }
}
