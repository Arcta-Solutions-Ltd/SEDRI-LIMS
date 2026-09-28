using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the wet prep (also known as a vaginal wet mount or vaginal smear) test report section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property, grids with name and fields with labels, values, columns and order.
/// </summary>
internal class WetPrepSection : IDefinition
{
    /// <summary>
    /// Gets the wet prep test report section configuration.
    /// Fields include count of red and white blood cell results from the test.
    /// Grids include results of parasites found from the test.
    /// </summary>
    /// <returns>JSON string containing wet prep test report section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "WetPrepSection",
                "Description": "@RepWet@",
                "HeadingText": "@RepWet@",
                "Format": "DoubleColumnWithGridTwo",
                "Type": "layout",
                "DataSection": "WetPrepDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesWbcB@",
                        "Value": "wbcwetprep",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@TesRbcB@",
                        "Value": "rbcwetprep",
                        "Column": 2,
                        "Order": 2
                    }
                ],
                "Grids": [
                    {
                        "Name": "ParasiteGrid"
                    }
                ]
            }
            """;
    }
}
