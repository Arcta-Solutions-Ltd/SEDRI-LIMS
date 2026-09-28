using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Factory for creating report page definitions based on a definition name.
/// </summary>
internal class ReportPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an <see cref="IDefinition"/> instance corresponding to the specified page definition name.
    /// </summary>
    /// <param name="definitionName">The name of the page definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> instance if the name matches a known page; otherwise, null.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "approvereportpage" => new ApproveReportPageConfig(),
            "batchapprovereportpage" => new BatchApproveReportPageConfig(),
            "batchprintpage" => new BatchPrintPageConfig(),
            "batchpublishpage" => new BatchPublishPageConfig(),
            "batchrejectreportpage" => new BatchRejectReportPageConfig(),
            "specimenreportpage" => new SpecimenReportPageConfig(),
            "cultureprintselectorpage" => new CulturePrintSelectorPageConfig(),
            "unapprovereportpage" => new UnapproveReportPageConfig(),
            _ => null,
        };
    }
}
