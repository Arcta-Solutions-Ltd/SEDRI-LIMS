using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Factory for creating report form definitions based on a definition name.
/// </summary>
internal class ReportFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an <see cref="IDefinition"/> instance corresponding to the specified form definition name.
    /// </summary>
    /// <param name="definitionName">The name of the form definition to create.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> instance if the name matches a known form; otherwise, null.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "approvereportform" => new ApproveReportFormConfig(),
            "batchapprovereportform" => new BatchApproveReportFormConfig(),
            "batchprintform" => new BatchPrintFormConfig(),
            "batchpublishform" => new BatchPublishFormConfig(),
            "batchrejectreportform" => new BatchRejectReportFormConfig(),
            "cultureprintselectorform" => new CulturePrintSelectorFormConfig(),
            "specimenreportform" => new SpecimenReportFormConfig(),
            "unapprovereportform" => new UnapproveReportFormConfig(),
            _ => null,
        };
    }
}
