using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Catalase Section.
/// The catalase test is a biochemical test that identifies organisms that produce the enzyme catalase.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class CatalaseSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Catalase Section.
    /// Fields include catalaseresultId, which references a ListItem containing the catalase test result.
    /// </summary>
    /// <returns>JSON string containing Catalase Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CatalaseSection",
                "Description": "@TesCatA@",
                "HeadingText": "@TesCatA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "CatalaseDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "catalaseresultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
