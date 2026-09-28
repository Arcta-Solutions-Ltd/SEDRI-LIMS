using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides the configuration for the UnapproveReport UI event.
/// </summary>
internal class UnapproveReportUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the UI event.
    /// </summary>
    /// <returns>A JSON string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'unapprovereportuievent',
            description: 'Unapprove report',
            type: 'form',
            action: 'unapprovereportform'
        }";

        return newEvent;
    }
}
