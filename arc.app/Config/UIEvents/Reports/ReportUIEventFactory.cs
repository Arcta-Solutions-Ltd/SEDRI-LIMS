using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Factory for creating report UI event definitions based on a definition name.
/// </summary>
internal class ReportUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an IDefinition instance corresponding to the specified definition name.
    /// </summary>
    /// <param name="definitionName">Name of the UI event definition to create.</param>
    /// <returns>An IDefinition instance if the name matches; otherwise, null.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "approvereportuievent" => new ApproveReportUIEventConfig(),
            "batchapprovereportuievent" => new BatchApproveReportUIEventConfig(),
            "batchprintuievent" => new BatchPrintUIEventConfig(),
            "batchpublishuievent" => new BatchPublishUIEventConfig(),
            "batchrejectreportuievent" => new BatchRejectReportUIEventConfig(),
            "cultureprintselectoruievent" => new CulturePrintSelectorUIEventConfig(),
            "showreportonscreenuievent" => new ShowReportOnscreenUIEventConfig(),
            "specimenreportuievent" => new SpecimenReportUIEventConfig(),
            "unapprovereportuievent" => new UnapproveReportUIEventConfig(),
            _ => null,
        };
    }
}
