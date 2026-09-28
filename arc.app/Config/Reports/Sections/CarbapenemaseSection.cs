using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Carbapenemase Section.
/// Carbapenemase tests detect the presence of carbapenemase genes in bacteria.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class CarbapenemaseSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Carbapenemase Section.
    /// Fields include carbapenemaseresultId, which references a ListItem containing the carbapenemase test result.
    /// </summary>
    /// <returns>JSON string containing Carbapenemase Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CarbapenemaseSection",
                "Description": "@TesCarC@",
                "HeadingText": "@TesCarC@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "CarbapenemaseDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "carbapenemaseresultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
