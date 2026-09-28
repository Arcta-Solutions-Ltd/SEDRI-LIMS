using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a single column section format with one column.
/// This format provides a layout with a single column for displaying fields.
/// </summary>
internal class SingleColumnOneConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the single column one format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a single column layout.</returns>
    public string Get()
    {
        return """
            {
                "Name": "SingleColumnOne",
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
                    {
                        "left": 20,
                        "width": 520,
                        "labelwidth": 80
                    }
                ]
            }
            """;
    }
}
