using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Dipstick data section.
/// This data section provides the available fields for urine dipstick test results including
/// pH, protein, glucose, leucocytes, specific gravity, ketones, blood, and nitrites.
/// </summary>
internal class DipstickDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Dipstick data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DipstickDataSection",
                "Title": "@RepCel@",
                "Fields": [
                    {
                        "Label": "@TesPh@",
                        "Value": "phId"
                    },
                    {
                        "Label": "@TesProA@",
                        "Value": "proteinId"
                    },
                    {
                        "Label": "@TesGluA@",
                        "Value": "glucoseId"
                    },
                    {
                        "Label": "@TesLeu@",
                        "Value": "leucocytesId"
                    },
                    {
                        "Label": "@TesSpe@",
                        "Value": "specificGravityId"
                    },
                    {
                        "Label": "@TesKet@",
                        "Value": "ketonesId"
                    },
                    {
                        "Label": "@GenBlo@",
                        "Value": "bloodId"
                    },
                    {
                        "Label": "@TesNit@",
                        "Value": "nitritesId"
                    }
                ]
            }
            """;
    }
}
