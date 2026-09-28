using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Microscopy Section.
/// A microscopy test is a laboratory procedure that examines a sample under a microscope to identify its components.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property, grids with crystal and fields with labels, values, columns and order.
/// </summary>
internal class MicroscopySection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Microscopy Section.
    /// Fields include epithelium cell, yeast and bacteria results from the test.
    /// Grids include results of crystals and casts found from the test, placed as two separate
    /// tables by a format with two grid positions.
    /// </summary>
    /// <returns>JSON string containing Microscopy Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "MicroscopySection",
                "Description": "@TesMicA@",
                "HeadingText": "@TesMicA@",
                "Format": "DoubleColumnWithTwoGrids",
                "Type": "layout",
                "DataSection": "MicroscopyDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesEpiA@",
                        "Value": "epitheliumId",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@GenYea@",
                        "Value": "yeastId",
                        "Column": 1,
                        "Order": 2
                    },
                    {
                        "Label": "@GenBacA@",
                        "Value": "bacteriaId",
                        "Column": 2,
                        "Order": 3
                    }
                ],
                "Grids": [
                    {
                        "Name": "CrystalGrid"
                    },
                    {
                        "Name": "CastGrid"
                    }
                ]
            }
            """;
    }
}
