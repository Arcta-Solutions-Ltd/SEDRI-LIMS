using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Factory for creating report event definitions based on a definition name.
/// </summary>
internal class ReportEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an <see cref="IDefinition"/> instance corresponding to the specified event definition name.
    /// </summary>
    /// <param name="definitionName">The name of the event definition to create.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> instance if the name matches a known event; otherwise, null.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "approvereportevent" => new ApproveReportEventConfig(),
            "batchapprovereportevent" => new BatchApproveReportEventConfig(),
            "batchrejectreportevent" => new BatchRejectReportEventConfig(),
            "cultureprintselector" => new CulturePrintSelectorEventConfig(),
            "specimenreport" => new SpecimenReportEventConfig(),
            "unapprovereportevent" => new UnapproveReportEventConfig(),
            _ => null,
        };
    }
}
