using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a single column section format with two layout variant.
/// This format provides a layout with a single column for displaying fields with a wider label width.
/// </summary>
internal class SingleColumnTwoConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the single column two format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a single column layout.</returns>
    public string Get()
    {
        return """
            {
                "Name": "SingleColumnTwo",
                "Type": "SingleFieldColumnWithSeparateHeading",
                "Description": "@RepSin@",
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
                    { "left": 20, "width": 440, "labelwidth": 160 }
                ]
            }
            """;
    }
}
