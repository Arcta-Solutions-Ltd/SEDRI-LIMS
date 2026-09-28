using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Gram Stain Section.
/// A Gram stain is a test that identifies bacteria in a sample by staining it and examining it under a microscope.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property, grids with organisms and fields with labels, values, columns and order.
/// </summary>
internal class GramStainSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Gram Stain Section.
    /// Fields include white blood cell and epithelial cell results from the test.
    /// Grids include results of organisms found from the test.
    /// </summary>
    /// <returns>JSON string containing Gram Stain Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "GramStainSection",
                "Description": "@RepGra@",
                "HeadingText": "@RepGra@",
                "Format": "DoubleColumnWithGridOne",
                "Type": "layout",
                "DataSection": "GramStainDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesWbcB@",
                        "Value": "wbc",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@TesEpi@",
                        "Value": "Epicells",
                        "Column": 2,
                        "Order": 2
                    }
                ],
                "Grids": [
                    {
                        "Name": "OrganismGrid"
                    }
                ]
            }
            """;
    }
}
