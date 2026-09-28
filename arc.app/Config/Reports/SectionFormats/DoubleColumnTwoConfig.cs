using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a double column section format with two layout variant.
/// This format provides a layout with two columns for displaying fields side by side with row headings.
/// </summary>
internal class DoubleColumnTwoConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the double column two format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a two-column layout and row headings.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DoubleColumnTwo",
                "Type": "DoubleFieldColumnWithRowHeading",
                "Description": "@RepTwoA@",
                "Heading": [
                    {
                        "Line": 1,
                        "Left": 20,
                        "Text": "@RepCel@",
                        "FontSize": 12,
                        "Bold": true
                    }
                ],
                "Columns": [
                    {
                        "left": 100,
                        "width": 220,
                        "labelwidth": 100
                    },
                    {
                        "left": 330,
                        "width": 220,
                        "labelwidth": 100
                    }
                ]
            }
            """;
    }
}
