using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Gram Culture Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property, grids with organisms and fields with labels, values, columns and order.
/// </summary>
internal class GramCultureSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Gram Culture Section.
    /// </summary>
    /// <returns>JSON string containing Gram Culture Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "GramCultureSection",
                "Description": "@RepGra@",
                "HeadingText": "@RepGra@",
                "Format": "DoubleColumnWithGridOne",
                "Type": "layout",
                "DataSection": "GramCultureDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesEpi@",
                        "Value": "gcEpicells",
                        "Column": 1,
                        "Order": 1
                    }
                ],
                "Grids": [
                    {
                        "Name": "GramCultureTable"
                    }
                ]
            }
            """;
    }
}
