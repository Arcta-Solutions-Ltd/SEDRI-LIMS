using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for displaying the Approval Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section,
/// dynamic property.
/// </summary>
internal class ApprovalSection : IDefinition
{
    /// <summary>
    /// Gets the Approval Section configuration.
    /// Fields includes names of approvers and the dates of approval.
    /// </summary>
    /// <returns>JSON string containing the Approval Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "ApprovalSection",
                "Description": "@SpeAppA@",
                "HeadingText": "@SpeAppA@",
                "Format": "DoubleColumnOne",
                "Type": "layout",
                "DataSection": "ApprovalDataSection",
                "Fields": [
                    {
                        "Label": "@SpeAppB@",
                        "Value": "SubmittedBy",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@SpeAppC@",
                        "Value": "ApprovedBy",
                        "Column": 1,
                        "Order": 2
                    },
                    {
                        "Label": "@RepPreA@",
                        "Value": "SubmittedDate",
                        "Column": 2,
                        "Order": 3
                    },
                    {
                        "Label": "@RepPreA@",
                        "Value": "ApprovedDate",
                        "Column": 2,
                        "Order": 4
                    }
                ]
            }
            """;
    }
}
