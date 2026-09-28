using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Auramine Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class AuramineSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Auramine Section.
    /// Fields include AuramineId, which references a ListItem containing the auramine test result.
    /// </summary>
    /// <returns>JSON string containing Auramine Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "AuramineSection",
                "Description": "@RepAur@",
                "HeadingText": "@RepAur@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "AuramineDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "AuramineId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
