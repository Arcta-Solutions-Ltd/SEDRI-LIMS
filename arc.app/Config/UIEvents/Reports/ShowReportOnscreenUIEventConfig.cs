using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration for displaying a report on screen.
/// </summary>
internal class ShowReportOnscreenUIEventConfig : IDefinition
{

    /// <summary>
    /// Gets the JSON definition for the show-report-on-screen UI event.
    /// </summary>
    /// <returns>
    /// A JSON string defining the UI event with name, description, type, and action.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'showreportonscreenuievent',
                        description: 'Print report',
                        type: 'printreport',
                        action: 'specimenreportview'
                    }";

        return newEvent;
    }
}
