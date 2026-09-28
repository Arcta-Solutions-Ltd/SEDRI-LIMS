using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Pregnancy Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property and fields with labels, values, columns and order.
/// </summary>
internal class PregnancySection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Pregnancy Section.
    /// Fields include a pregnancyId, which references a ListItem containing the pregnancy test result.
    /// </summary>
    /// <returns>JSON string containing Pregnancy Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "PregnancySection",
                "Description": "@TesPreA@",
                "HeadingText": "@TesPreA@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "PregnancyDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesTesA@",
                        "Value": "pregnancyId",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
