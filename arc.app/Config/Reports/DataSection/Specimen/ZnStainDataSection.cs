using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the ZN (Ziehl-Neelsen) Stain data section.
/// This data section provides the available fields for ZN stain test results including AFB (Acid-Fast Bacilli) quantity.
/// </summary>
internal class ZnStainDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the ZN Stain data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "ZnStainDataSection",
                "Title": "@RepZns@",
                "Fields": [
                    {
                        "Label": "@TesAfb@",
                        "Value": "AFBQuantity"
                    }
                ]
            }
            """;
    }
}
