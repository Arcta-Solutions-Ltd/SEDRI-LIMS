using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Preculture Results Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format,
/// data section property and fields with labels, values, columns and order.
/// </summary>
internal class PrecultureResultsSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Preculture Results Section.
    /// Fields include a SpecimenAppearance, which states the specimen's appearance.
    /// </summary>
    /// <returns>JSON string containing Preculture Results Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "PrecultureResultsSection",
                "Description": "@RepPre@",
                "HeadingText": "@SpeDir@",
                "Format": "DoubleColumnOne",
                "Type": "layout",
                "DataSection": "PrecultureResultsDataSection",
                "Fields": [
                    {
                        "Label": "@RepApp@",
                        "Value": "SpecimenAppearance",
                        "Column": 1,
                        "Order": 1
                    }
                ]
            }
            """;
    }
}
