using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Cell Count Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class CellCountSection : IDefinition
{
    public string Get()
    {
        return """
            {
                "Name": "CellCountSection",
                "Description": "@TesCel@",
                "HeadingText": "@RepCel@",
                "Format": "DoubleColumnFour",
                "Type": "layout",
                "DataSection": "CellCountDataSection",
                "Dynamic": true,
                "Fields": [
                    {
                        "Label": "@TesWbc@",
                        "Value": "CCWbc",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@TesRbc@",
                        "Value": "CCRbc",
                        "Column": 1,
                        "Order": 2
                    },
                    {
                        "Label": "@TesWbcA@",
                        "Value": "WbcQualitative",
                        "Column": 1,
                        "Order": 3
                    },
                    {
                        "Label": "@TesRbcA@",
                        "Value": "RbcQualitative",
                        "Column": 2,
                        "Order": 4
                    },
                    {
                        "Label": "@TesPol@",
                        "Value": "Polymorphonuclear",
                        "Column": 2,
                        "Order": 5
                    },
                    {
                        "Label": "@TesMon@",
                        "Value": "Mononuclear",
                        "Column": 2,
                        "Order": 6
                    }
                ]
            }
            """;
    }
}
