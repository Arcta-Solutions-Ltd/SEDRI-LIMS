using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Culture Result Section.
/// A culture test is a laboratory test that analyzes a sample of tissue, blood, urine,
/// or other bodily fluid to identify the presence of bacteria, fungi, or other microorganisms.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section
/// grids with organisms and fields with labels, values, columns and order.
/// </summary>
internal class CultureResultSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Culture Results Section.
    /// Fields include OrganismWithGrowth which details culture type, growth, quantity, and optional organism on the report heading.
    /// </summary>
    /// <returns>JSON string containing Culture Results Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CultureResultSection",
                "Description": "@RepCul@",
                "HeadingText": "@RepCul@",
                "Format": "DynamicSingleColumnOne",
                "Type": "layout",
                "DataSection": "CultureResultDataSection",
                "Fields": [
                    {
                        "Label": "@RepOrg@",
                        "Value": "OrganismWithGrowth",
                        "Column": 1,
                        "Order": 2
                    }
                ]
            }
            """;
    }
}
