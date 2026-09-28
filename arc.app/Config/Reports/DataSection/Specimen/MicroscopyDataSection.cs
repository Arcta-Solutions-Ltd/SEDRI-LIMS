using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Microscopy data section.
/// This data section provides the available fields for microscopy results and includes grids for crystal and cast data.
/// </summary>
internal class MicroscopyDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Microscopy data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields and grids.</returns>
    public string Get()
    {
        return """
            {
                "Name": "MicroscopyDataSection",
                "Title": "@TesMicA@",
                "Fields": [
                    {
                        "Label": "@TesEpiA@",
                        "Value": "epitheliumId"
                    },
                    {
                        "Label": "@GenYea@",
                        "Value": "yeastId"
                    },
                    {
                        "Label": "@GenBacA@",
                        "Value": "bacteriaId"
                    }
                ],
                "Grids": [
                    {
                        "name": "CrystalGrid",
                        "description": "Crystal",
                        "data": "crystal"
                    },
                    {
                        "name": "CastGrid",
                        "description": "Cast",
                        "data": "cast"
                    }
                ]
            }
            """;
    }
}
