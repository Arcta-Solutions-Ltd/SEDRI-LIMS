using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration for batch rejecting reports.
/// </summary>
internal class BatchRejectReportUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject report UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
            name: 'batchrejectreportuievent',
            description: 'Batch reject report',
            type: 'form',
            action: 'batchrejectreportform'
        }";

        return newEvent;
    }
}
