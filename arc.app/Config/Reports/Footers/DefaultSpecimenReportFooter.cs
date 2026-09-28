using arc.app.Common;

namespace arc.app.Config.Reports.Footers;

/// <summary>
/// The default specimen report footer configuration.
/// </summary>
internal class DefaultSpecimenReportFooter : IDefinition
{
    /// <summary>
    /// Gets the default specimen report footer configuration.
    /// </summary>
    /// <returns>JSON string representing the default specimen report footer configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DefaultSpecimenReportFooter",
                "Description": "@RepDef@",
                "Lines": [
                    {
                        "Line": 1,
                        "Left": 20,
                        "Text": "@RepIfy@",
                        "FontSize": 11
                    },
                    {
                        "Line": 2,
                        "Left": 20,
                        "Calc": "PrintedDate",
                        "FontSize": 11
                    },
                    {
                        "Line": 2,
                        "Left": 530,
                        "Calc": "Pages",
                        "FontSize": 11
                    }
                ]
            }
            """;
    }
}
