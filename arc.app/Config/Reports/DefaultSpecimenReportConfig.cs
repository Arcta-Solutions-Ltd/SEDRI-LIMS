using arc.app.Common;

namespace arc.app.Config.Reports;

/// <summary>
/// Configuration for the default specimen report.
/// </summary>
internal class DefaultSpecimenReportConfig : IDefinition
{
    /// <summary>
    /// Returns the configuration for the default specimen report in JSON format.
    /// </summary>
    /// <returns>A JSON string representing the configuration of the default specimen report.</returns>
    public string Get()
    {
        return """
            {
                "Name": "defaultspecimenreport",
                "Title": "@RepDefA@",
                "Header": "DefaultSpecimenReportHeader",
                "Footer": "DefaultSpecimenReportFooter",
                "IncludeAlerts": true,
                "SectionSource": [
                    { "Name": "MainSections", "Source": "Main" },
                    { "Name": "OrganismSections", "Source": "Organism" },
                    { "Name": "FinalSections", "Source": "Main" }
                ],
                "MainSections": [
                    "patientdetailssection",
                    "locationsection",
                    "specimencommentssection",
                    "cellcountsection",
                    "gramstainsection",
                    "znstainsection",
                    "wetprepsection",
                    "auraminesection",
                    "dipsticksection",
                    "hpyloriantigensection",
                    "jevserologysection",
                    "kohprepsection",
                    "microscopysection",
                    "pregnancysection",
                    "biochemistrysection",
                    "wrightsstainsection",
                    "indiainksection"
                ],
                "OrganismSections": [
                    "cultureresultsection",
                    "bloodandbottleweightsection",
                    "culturedatesection",
                    "esblsection",
                    "betalactamasesection",
                    "carbapenemasesection",
                    "gramculturesection",
                    "catalasesection",
                    "oxidasesection",
                    "apipanelsection",
                    "organismlistsection",
                    "culturecommentssection",
                    "astsection"
                ],
                "FinalSections": [
                    "approvalsection"
                ],
                "Configurable": "No"

            }
            """;
    }
}
