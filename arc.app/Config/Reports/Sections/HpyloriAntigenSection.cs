using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the H.pylori Antigen Section.
/// An H. pylori antigen test is a stool test that checks for proteins (antigens) associated with the H. pylori bacteria.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property and fields with labels, values, columns and order.
/// </summary>
internal class HpyloriAntigenSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the H.pylori Antigen Section.
    /// Fields include a antResultId, which references a ListItem containing the H.pylori antigen test result.
    /// </summary>
    /// <returns>JSON string containing H.pylori Antigen Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "HpyloriAntigenSection",
                "Description": "@TesHpyA@",
                "HeadingText": "@TesHpyA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "HpyloriAntigenDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "antResultId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
