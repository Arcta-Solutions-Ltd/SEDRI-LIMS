using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a single-column table section format.
/// Used for full-width tables such as specimen and culture comment sections.
/// </summary>
internal class TableSingleColumnConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the single-column table format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with one full-width grid.</returns>
    public string Get()
    {
        return """
            {
                "Name": "TableSingleColumn",
                "Type": "Table",
                "Grids": [
                    {
                        "left": 20,
                        "width": "550"
                    }
                ]
            }
            """;
    }
}
