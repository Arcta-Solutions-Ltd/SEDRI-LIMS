using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a double column section format with one layout variant.
/// This format provides a layout with two columns for displaying fields side by side.
/// </summary>
internal class DoubleColumnOneConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the double column one format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a two-column layout.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DoubleColumnOne",
                "Type": "DoubleFieldColumn",
                "Description": "@RepTwo@",
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
                        "left": 20,
                        "width": 240,
                        "labelwidth": 80
                    },
                    {
                        "left": 300,
                        "width": 240,
                        "labelwidth": 80
                    }
                ]
            }
            """;
    }
}
