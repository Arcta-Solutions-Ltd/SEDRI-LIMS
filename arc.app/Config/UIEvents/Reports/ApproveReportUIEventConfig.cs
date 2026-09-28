using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides the configuration for the ApproveReport UI event.
/// </summary>
internal class ApproveReportUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the ApproveReport UI event.
    /// </summary>
    /// <returns>A JSON string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'approvereportuievent',
            description: 'Approve report',
            type: 'form',
            action: 'approvereportform'
        }";

        return newEvent;
    }
}
