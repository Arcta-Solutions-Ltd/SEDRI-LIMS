using arc.app.Common;
using arc.app.Config.Queries.Reports;

namespace arc.app.Config.Queries;

/// <summary>
/// Factory class responsible for creating report-related query definition instances based on a string identifier.
/// </summary>
internal class ReportQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of an <see cref="IDefinition"/> implementation corresponding to the provided query definition name.
    /// </summary>
    /// <param name="definitionName">The name of the query definition to instantiate.</param>
    /// <returns>
    /// A concrete <see cref="IDefinition"/> matching the provided name, or <c>null</c> if no match is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "antibiogramquery" => new AntibiogramQuery(),
            "approvereportformquery" => new ApproveReportFormQuery(),
            "approvedreportslistquery" => new ApprovedReportsListQuery(),
            "cellcountbyspecimenid" => new CellCountBySpecimenIdQuery(),
            "editcultureprintselectionquery" => new EditCulturePrintSelectionQuery(),
            "admissionreportlist" => new AdmissionReportListQuery(),
            "patientreportlist" => new PatientReportListQuery(),
            "requestreportlist" => new RequestReportListQuery(),
            "resistantantibioticquery" => new ResistantAntibioticQuery(),
            "resistantorganismquery" => new ResistantOrganismQuery(),
            "reportcomments" => new ReportCommentsQuery(),
            "reportcontents" => new ReportContentsQuery(),
            "reportfiltercontents" => new ReportFilterContentsQuery(),
            "reportlist" => new ReportListQuery(),
            "specimenandpatientrecord" => new SpecimenandPatientRecordQuery(),
            "specimenrecordreport" => new SpecimenRecordReportQuery(),
            "unapprovedreportslistquery" => new UnapprovedReportsListQuery(),
            "unapprovedreportslistbyidquery" => new UnapprovedReportsListByIdQuery(),
            _ => null,
        };
    }
}
