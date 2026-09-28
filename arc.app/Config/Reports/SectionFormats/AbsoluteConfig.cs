using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for an absolute section format.
/// This format uses absolute positioning with lines for custom layout control.
/// </summary>
internal class AbsoluteConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the absolute format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration for absolute positioning.</returns>
    public string Get()
    {
        return """
            {
                "Name": "Absolute",
                "Description": "@GenAbs@",
                "Type": "Lines"
            }
            """;
    }
}
