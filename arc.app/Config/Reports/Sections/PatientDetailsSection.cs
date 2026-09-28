using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Defines the configuration for the Patient Details Section.
/// It implements the IDefinition interface and provides the report configuration as a JSON string.
/// The configuration includes properties such as the section name, description, heading text, format, data section
/// property, and fields with labels, values, columns and order.
/// </summary>
internal class PatientDetailsSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Patient Details Section.
    /// Fields include patient name, gender, date of birth and age.
    /// </summary>
    /// <returns>JSON string containing Patient Details Section configuration.</returns>
    public string Get()
    {
        return """
            {
                "Name": "PatientDetailsSection",
                "Description": "@PatDet@",
                "HeadingText": "@PatDet@",
                "Format": "DoubleColumnOne",
                "Type": "layout",
                "DataSection": "PatientDetailsDataSection",
                "Fields": [
                    {
                        "Label": "@RepRef@",
                        "Value": "PatientRef",
                        "Column": 1,
                        "Order": 1
                    },
                    {
                        "Label": "@PatFir@",
                        "Value": "FirstName",
                        "Column": 1,
                        "Order": 2
                    },
                    {
                        "Label": "@PatDat@",
                        "Value": "DateOfBirth",
                        "Column": 1,
                        "Order": 3
                    },
                    {
                        "Label": "@PatGenA@",
                        "Value": "Gender",
                        "Column": 2,
                        "Order": 4
                    },
                    {
                        "Label": "@PatSurA@",
                        "Value": "Surname",
                        "Column": 2,
                        "Order": 5
                    },
                    {
                        "Label": "@PatAgeA@",
                        "Value": "Age",
                        "Column": 2,
                        "Order": 6
                    },
                    {
                        "Label": "@PatAgeB@",
                        "Value": "AgeMonths",
                        "Column": 2,
                        "Order": 7
                    }
                ]
            }
            """;
    }
}
