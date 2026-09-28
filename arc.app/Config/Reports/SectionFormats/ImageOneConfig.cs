using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Defines the configuration for an image section format.
/// This format provides a layout for displaying images in reports.
/// </summary>
internal class ImageOneConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the image one format configuration.
    /// </summary>
    /// <returns>A JSON string containing the format configuration for displaying images.</returns>
    public string Get()
    {
        return """
            {
                "Name": "ImageOne",
                "Type": "ReportImage",
                "Columns": [
                    { "left": 160, "width": 255, "labelwidth": 80 }
                ]
            }
            """;
    }
}
