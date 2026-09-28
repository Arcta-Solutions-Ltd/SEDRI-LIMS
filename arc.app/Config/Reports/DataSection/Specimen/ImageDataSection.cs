using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Image data section.
/// This data section provides the available fields for displaying accreditation images.
/// </summary>
internal class ImageDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Image data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "ImageDataSection",
                "Title": "@RepImg@",
                "Fields": [
                    {
                        "Label": "Accreditation",
                        "Value": "accreditationimage"
                    }
                ]
            }
            """;
    }
}
