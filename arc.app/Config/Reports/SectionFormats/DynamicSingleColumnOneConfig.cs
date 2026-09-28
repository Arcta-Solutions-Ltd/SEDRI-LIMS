using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a dynamic single column section format.
/// This format provides a layout with a single column that can dynamically adjust based on content.
/// </summary>
internal class DynamicSingleColumnOneConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the dynamic single column one format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a dynamic single column layout.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DynamicSingleColumnOne",
                "Type": "DynamicSingleColumnOne",
                "Description": "@RepDyn@",
                "Columns": [
                    { "left": 100, "width": 440, "labelwidth": 100 }
                ]
            }
            """;
    }
}
