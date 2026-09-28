using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the AST Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class AstSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the AST Section.
    /// Fields include comments for AST.
    /// </summary>
    /// <returns>JSON string containing AST Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "astSection",
                "Description": "@RepAst@",
                "Format": "SingleColumnOne",
                "Type": "layout",
                "DataSection": "AstDataSection",
                "Fields": [
                    {
                        "Label": "@AstCom1@",
                        "Value": "astcommentone",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@AstCom2@",
                        "Value": "astcommenttwo",
                        "Column": 1,
                        "Order": 2
                    }
                ]
            }
            """;
    }
}
