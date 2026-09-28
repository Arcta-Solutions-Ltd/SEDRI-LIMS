using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Oxidase Test Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property, and fields with labels, values, columns and order.
/// </summary>
internal class OxidaseSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Oxidase Test Section.
    /// Fields include a oxidaseresultId, which references a ListItem containing the oxidase test result.
    /// </summary>
    /// <returns>JSON string containing Oxidase Test Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "OxidaseSection",
                "Description": "@TesOxiA@",
                "HeadingText": "@TesOxiA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "OxidaseDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "oxidaseresultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
