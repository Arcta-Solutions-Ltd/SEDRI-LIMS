using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a table section format with two grid layout.
/// This format provides a layout with a single grid/table for displaying tabular data.
/// </summary>
internal class TableTwoConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the table two format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a single grid layout.</returns>
    public string Get()
    {
        return """
            {
                "Name": "TableTwo",
                "Type": "Table",
                "Grids": [
                    { "left": 20, "width": "183|183|184" }
                ]
            }
            """;
    }
}
