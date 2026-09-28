using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Biochemistry Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class BiochemistrySection : IDefinition
{
    public string Get()
    {
        return """
            {
                "Name": "BiochemistrySection",
                "Description": "@TesBioA@",
                "HeadingText": "@TesBioA@",
                "Format": "DoubleColumnOne",
                "Type": "layout",
                "DataSection": "BiochemistryDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesGlu@",
                        "Value": "glucose",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@TesPro@",
                        "Value": "protein",
                        "Column": 2,
                        "Order": 2
                    }
                ]
            }
            """;
    }
}
