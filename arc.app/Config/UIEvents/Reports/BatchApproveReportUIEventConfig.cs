using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration for batch approving reports.
/// </summary>
internal class BatchApproveReportUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve report UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
            name: 'batchapprovereportuievent',
            description: 'Batch approve report',
            type: 'form',
            action: 'batchapprovereportform'
        }";

        return newEvent;
    }
}
