using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a double column section format with three layout variant.
/// This format provides a layout with two columns for displaying fields side by side with row headings.
/// </summary>
internal class DoubleColumnThreeConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the double column three format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a two-column layout and row headings.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DoubleColumnThree",
                "Type": "DoubleFieldColumnWithRowHeading",
                "Description": "@RepTwoC@",
                "Heading": [
                    {
                        "Line": 1,
                        "Left": 20,
                        "Text": "@PatDet@",
                        "FontSize": 14,
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
                ]
            }
            """;
    }
}
