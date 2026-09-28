using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Wright's stain test report section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property and fields with labels, values, columns and order.
/// </summary>
internal class WrightsStainSection : IDefinition
{
    /// <summary>
    /// Gets the Wright's stain test stain test report section configuration.
    /// Fields include a wrightsstainresultId, which references a ListItem containing the Wright's stain test result.
    /// </summary>
    /// <returns>JSON string containing Wright's stain test report section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "WrightsStainSection",
                "Description": "@TesWriA@",
                "HeadingText": "@TesWriA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "WrightsStainDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "wrightsstainresultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
