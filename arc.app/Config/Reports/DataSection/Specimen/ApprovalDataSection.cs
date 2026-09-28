using arc.app.Common;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Defines the configuration for the Approval data section.
/// This data section provides the available fields for approval information including
/// submitted by, approved by, and their respective dates.
/// </summary>
internal class ApprovalDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Approval data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "ApprovalDataSection",
                "Title": "@SpeAppA@",
                "Fields": [
                    {
                        "Label": "@SpeAppB@",
                        "Value": "SubmittedBy"
                    },
                    {
                        "Label": "@SpeAppC@",
                        "Value": "ApprovedBy"
                    },
                    {
                        "Label": "@SpeSubDat@",
                        "Value": "SubmittedDate"
                    },
                    {
                        "Label": "@SpeAppDat@",
                        "Value": "ApprovedDate"
                    }
                ]
            }
            """;
    }
}
