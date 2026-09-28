using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the API Panel Section.
/// An API panel lab test, or Analytical Profile Index test, is a biochemical test kit that identifies bacteria and yeast.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class ApiPanelSection : IDefinition
{
    /// <summary>
    /// Gets the API Panel Section configuration.
    /// Fields includes the results of the API panel test.
    /// </summary>
    /// <returns>JSON string containing the API Panel Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "ApiPanelSection",
                "Description": "@TesApiB@",
                "HeadingText": "@TesApiB@",
                "Format": "DoubleColumnOne",
                "Type": "layout",
                "DataSection": "ApiPanelDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@SpeApi@",
                        "Value": "APIIDPanel",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@SpeIdA@",
                        "Value": "PercentageID",
                        "Column": 1,
                        "Order": 2
                    },
                    {
                        "Label": "@SpeId@",
                        "Value": "IdProfile",
                        "Column": 2,
                        "Order": 3
                    }
                ]
            }
            """;
    }
}
