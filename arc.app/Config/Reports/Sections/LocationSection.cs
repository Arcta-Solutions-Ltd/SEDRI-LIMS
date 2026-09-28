using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Location Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format,
/// data section property and fields with labels, values, columns and order.
/// </summary>
internal class LocationSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Location Section.
    /// Fields include fully qualified location name.
    /// </summary>
    /// <returns>JSON string containing location section report configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "LocationSection",
                "Description": "@GenLoc@",
                "HeadingText": "",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "LocationDataSection",
                "Fields": [
                    {
                        "Label": "@GenLoc@",
                        "Value": "fullyqualifiedname",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
