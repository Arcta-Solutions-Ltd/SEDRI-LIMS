using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Beta Lactamase Section.
/// A beta lactamase test detects the presence of beta lactamase, an enzyme produced by bacteria that makes them resistant to beta-lactam antibiotics.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class BetalactamaseSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Beta Lactamase Section.
    /// Fields include betalactamaseresultId, which references a ListItem containing the beta lactamase test result.
    /// </summary>
    /// <returns>JSON string containing Beta Lactamase Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "BetalactamaseSection",
                "Description": "@TesBetA@",
                "HeadingText": "@TesBetA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "BetalactamaseDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "betalactamaseresultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
