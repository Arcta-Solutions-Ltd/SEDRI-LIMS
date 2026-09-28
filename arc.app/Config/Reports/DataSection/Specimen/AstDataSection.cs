using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the AST (Antimicrobial Susceptibility Testing) data section.
/// This data section provides the available fields for AST test comments.
/// </summary>
internal class AstDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the AST data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
        {
            "Name": "AstDataSection",
            "Title": "@RepAst@",
            "Fields": [
                {
                    "Label": "@AstCom1@",
                    "Value": "astcommentone"
                },
                {
                    "Label": "@AstCom2@",
                    "Value": "astcommenttwo"
                }
            ]
        }
        """;
    }
}
