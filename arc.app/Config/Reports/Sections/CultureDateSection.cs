using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Culture Date Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section property.
/// </summary>
internal class CultureDateSection : IDefinition
{
    public string Get()
    {
        return """
            {
                "Name": "CultureDateSection",
                "Fields": [
                    {
                        "Label": "Result Date",
                        "Order": 1,
                        "Value": "positivedate",
                        "Column": 1
                    }
                ],
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "CultureDateDataSection",
                "Description": "Result Date",
                "HeadingText": "Result Details"
            }
            """;
    }
}
