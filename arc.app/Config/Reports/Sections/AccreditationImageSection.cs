using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Accreditation Image Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
public class AccreditationImageSection : IDefinition
{
    public string Get()
    {
        return """
            {
                "Name": "AccreditationImageSection",
                "Format": "ImageOne",
                "Type": "layout",
                "DataSection": "ImageDataSection",
                "Dynamic": false,
                "Fields": [
                    {
                        "Label": "@RepImg@",
                        "Value": "accreditationimage",
                        "Column": 1,
                        "Order": 1,
                        "Image": true,
                        "Width": 242.25,
                        "Height": 125.25,
                        "Format": "png"
                    }
                ]
            }
            """;
    }
}
