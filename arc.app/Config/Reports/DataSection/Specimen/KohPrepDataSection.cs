using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the KOH Prep data section.
/// This data section provides the available fields for KOH (potassium hydroxide) preparation test results.
/// </summary>
internal class KohPrepDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the KOH Prep data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "KohPrepDataSection",
                "Title": "@TesFunA@",
                "Fields": [
                    {
                        "Label": "@TesFunRes@",
                        "Value": "kohResultId"
                    },
                    {
                        "Label": "@TesFunPos@",
                        "Value": "KohFungalId"
                    }
                ]
            }
            """;
    }
}
