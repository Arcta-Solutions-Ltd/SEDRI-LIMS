using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Blood and Bottle Weight Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class BloodAndBottleWeightSection : IDefinition
{
    public string Get()
    {
        return """
            {
                "Name": "BloodAndBottleWeightSection",
                "Description": "@SpeBlo@",
                "HeadingText": "",
                "Format": "DoubleColumnFour",
                "Type": "layout",
                "DataSection": "BloodAndBottleWeightDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@SpeBot@",
                        "Value": "CultureBottleWeight",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@SpeBlo@",
                        "Value": "CultureBloodAndBottleWeight",
                        "Column": 1,
                        "Order": 2
                    }
                ]
            }
            """;
    }
}
