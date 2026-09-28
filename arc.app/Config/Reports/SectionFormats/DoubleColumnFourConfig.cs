using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a double column section format with four layout variant.
/// This format provides a layout with two columns for displaying fields side by side.
/// </summary>
internal class DoubleColumnFourConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the double column four format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a two-column layout.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DoubleColumnFour",
                "Type": "DoubleFieldColumn",
                "Description": "@RepTwoE@",
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
                    { "left": 20,  "width": 255, "labelwidth": 120 },
                    { "left": 315, "width": 255, "labelwidth": 120 }
                ]
            }
            """;
    }
}
