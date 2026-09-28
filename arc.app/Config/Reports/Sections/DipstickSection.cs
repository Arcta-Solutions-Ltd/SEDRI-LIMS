using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Dipstick Section.
/// A urine dipstick test measures the pH, protein, and leukocyte levels in urine.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property, grids with organisms and fields with labels, values, columns and order.
/// </summary>
internal class DipstickSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Dipstick Section.
    /// Fields include a phId, proteinId, glucoseId, leucocytesId, specificGravityId,
    /// ketonesId, bloodId, nitritesId which references a ListItem containing the dipstick test result.
    /// </summary>
    /// <returns>JSON string containing Dipstick Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DipstickSection",
                "Description": "@RepDip@",
                "HeadingText": "@RepDip@",
                "Format": "DoubleColumnFour",
                "Type": "layout",
                "DataSection": "DipstickDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesPh@",
                        "Value": "phId",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@TesProA@",
                        "Value": "proteinId",
                        "Column": 1,
                        "Order": 2
                    },
                    {
                        "Label": "@TesGluA@",
                        "Value": "glucoseId",
                        "Column": 1,
                        "Order": 3
                    },
                    {
                        "Label": "@TesLeu@",
                        "Value": "leucocytesId",
                        "Column": 1,
                        "Order": 4
                    },
                    {
                        "Label": "@TesSpe@",
                        "Value": "specificGravityId",
                        "Column": 2,
                        "Order": 5
                    },
                    {
                        "Label": "@TesKet@",
                        "Value": "ketonesId",
                        "Column": 2,
                        "Order": 6
                    },
                    {
                        "Label": "@GenBlo@",
                        "Value": "bloodId",
                        "Column": 2,
                        "Order": 7
                    },
                    {
                        "Label": "@TesNit@",
                        "Value": "nitritesId",
                        "Column": 2,
                        "Order": 8
                    }
                ]
            }
            """;
    }
}
