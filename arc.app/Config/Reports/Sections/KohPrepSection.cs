using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the KOH Preparation (aka Potassium Hydroxide Preparation) Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property and fields with labels, values, columns and order.
/// </summary>
internal class KohPrepSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the KOH Preparation Section.
    /// Fields include a kohResultId, KohFungalId which references a ListItem containing the result and fugus identified.
    /// </summary>
    /// <returns>JSON string containing KOH Preparation Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "KohPrepSection",
                "Description": "@TesFunA@",
                "HeadingText": "@TesFunA@",
                "Format": "DoubleColumnOne",
                "Type": "layout",
                "DataSection": "KohPrepDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@RepRes@",
                        "Value": "kohResultId",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@TesPos@",
                        "Value": "KohFungalId",
                        "Column": 2,
                        "Order": 2
                    }
                ]
            }
            """;
    }
}
