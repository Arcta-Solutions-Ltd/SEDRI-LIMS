using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the ESBL Section.
/// An extended-spectrum beta-lactamase (ESBL) test screens for bacteria that are resistant to many antibiotics.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property, grids with organisms and fields with labels, values, columns and order.
/// </summary>
internal class EsblSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the ESBL Section.
    /// Fields include a esblresultId, which references a ListItem containing the ESBL test result.
    /// </summary>
    /// <returns>JSON string containing ESBL Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "EsblSection",
                "Description": "@TesEsbA@",
                "HeadingText": "@TesEsbA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "EsblDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "esblresultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
