using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Ziehl-Neelsen (ZN) stain test report section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property and fields with labels, values, columns and order.
/// </summary>
internal class ZnStainSection : IDefinition
{
    /// <summary>
    /// Gets the Ziehl-Neelsen (ZN) stain test report section configuration.
    /// Fields include acid-fast bacilli (AFB) Quantity
    /// </summary>
    /// <returns>JSON string containing ZN stain report test section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "ZnStainSection",
                "Description": "@RepZns@",
                "HeadingText": "@RepZns@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "ZnStainDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesAfb@",
                        "Value": "AFBQuantity",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
