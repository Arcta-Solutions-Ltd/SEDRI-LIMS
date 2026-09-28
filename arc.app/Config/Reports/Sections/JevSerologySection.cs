using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the JEV Serology Section.
/// JEV serology is a blood test that looks for antibodies to the Japanese encephalitis virus (JEV).
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property and fields with labels, values, columns and order.
/// </summary>
internal class JevSerologySection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the JEV Serology Section.
    /// Fields include a jevserologyResultId, which references a ListItem containing the JEV serology test result.
    /// </summary>
    /// <returns>JSON string containing JEV Serology Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "JevSerologySection",
                "Description": "@TesJevA@",
                "HeadingText": "@TesJevA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "JevSerologyDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "jevserologyResultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
