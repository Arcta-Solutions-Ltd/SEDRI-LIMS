using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the India Ink Section.
/// The India ink test is a microscopic procedure that detects the presence of Cryptococcus neoformans in body fluids and tissues.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property and fields with labels, values, columns and order.
/// </summary>
internal class IndiaInkSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the India Ink Section.
    /// Fields include a IndiaInkResult and PositiveResult which contain the test results.
    /// </summary>
    /// <returns>JSON string containing India Ink Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "IndiaInkSection",
                "Description": "@RepInd@",
                "HeadingText": "@RepInd@",
                "Format": "DoubleColumnOne",
                "Type": "layout",
                "DataSection": "IndiaInkDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@RepRes@",
                        "Value": "IndiaInkResult",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@TesPos@",
                        "Value": "PositiveResult",
                        "Column": 2,
                        "Order": 2
                    }
                ]
            }
            """;
    }
}
