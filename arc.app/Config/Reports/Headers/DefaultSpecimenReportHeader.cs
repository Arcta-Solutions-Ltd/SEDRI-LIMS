using arc.app.Common;

namespace arc.app.Config.Reports.Headers;

/// <summary>
/// The default specimen report header configuration.
/// </summary>
internal class DefaultSpecimenReportHeader : IDefinition
{
    /// <summary>
    /// Gets the default specimen report header configuration.
    /// </summary>
    /// <returns>JSON string representing the default specimen report header configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "DefaultSpecimenReportHeader",
                "Description": "@RepDef@",
                "Lines": [
                    {
                        "Line": 1,
                        "Left": 20,
                        "Field": "LaboratoryName",
                        "FontSize": 16
                    },
                    {
                        "Line": 2,
                        "Left": 20,
                        "Text": "@RepMic@",
                        "Bold": true,
                        "FontSize": 20
                    },
                    {
                        "Line": 2,
                        "Left": 380,
                        "Field": "State",
                        "FontSize": 16
                    },
                    {
                        "Line": 3,
                        "Left": 20,
                        "Text": "@SpeSpeB@:"
                    },
                    {
                        "Line": 3,
                        "Left": 120,
                        "Field": "SpecimenType"
                    },
                    {
                        "Line": 3,
                        "Left": 380,
                        "Text": "@SpeColC@:"
                    },
                    {
                        "Line": 3,
                        "Left": 490,
                        "Field": "CollectionDate"
                    },
                    {
                        "Line": 4,
                        "Left": 20,
                        "Text": "@SpeAcc@:"
                    },
                    {
                        "Line": 4,
                        "Left": 120,
                        "Field": "AccessionNumber"
                    },
                    {
                        "Line": 4,
                        "Left": 380,
                        "Text": "@SpeColD@:"
                    },
                    {
                        "Line": 4,
                        "Left": 490,
                        "Field": "CollectionTime"
                    }
                ]
            }
            """;
    }
}
