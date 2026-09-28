using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for a table section format with one grid layout.
/// This format provides a layout with a single grid/table for displaying tabular data.
/// </summary>
internal class TableOneConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the table one format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration with a single grid layout.</returns>
    public string Get()
    {
        return """
            {
                "Name": "TableOne",
                "Type": "Table",
                "Grids": [
                    {
                        "left": 20,
                        "width": "200|100|140|80"
                    }
                ]
            }
            """;
    }
}
