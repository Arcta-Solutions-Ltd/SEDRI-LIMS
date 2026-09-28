using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Cell Count data section.
/// This data section provides the available fields for cell count measurements including
/// white blood cells, red blood cells, and their qualitative assessments.
/// </summary>
internal class CellCountDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Cell Count data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "CellCountDataSection",
                "Title": "@RepCel@",
                "Fields": [
                    {
                        "Label": "@TesWbc@",
                        "Value": "CCWbc"
                    },
                    {
                        "Label": "@TesRbc@",
                        "Value": "CCRbc"
                    },
                    {
                        "Label": "@TesWbcA@",
                        "Value": "WbcQualitative"
                    },
                    {
                        "Label": "@TesRbcA@",
                        "Value": "RbcQualitative"
                    },
                    {
                        "Label": "@TesPol@",
                        "Value": "Polymorphonuclear"
                    },
                    {
                        "Label": "@TesMon@",
                        "Value": "Mononuclear"
                    }
                ]
            }
            """;
    }
}
