using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the Breakpoint record view.
/// Displays breakpoint details at the top when View is clicked from the breakpoint listview.
/// </summary>
internal class BreakpointRecordViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the Breakpoint record view.
    /// </summary>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object that defines the record view layout for breakpoints,
    /// with breakpoint details at the top (standard region using breakpointbyidforedit).
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@BreMan@',
                        'name': 'breakpoints',
                        'type': 'recordview',
                        'refreshRecordOnEmbeddedSave': true,
                        buttons: [],
                        'regions': [
                            { 'id': 'breakpointdetails', 'type': 'standard', 'queryName': 'breakpointviewquery' },
                            { 'id': 'breakpointlines', 'type': 'listview', 'title': '@BreLin@', 'listViewName': 'breakpointlines' },
                            { 'id': 'breakpointapprovals', 'type': 'listview', 'title': '@BreAppHis@', 'listViewName': 'breakpointapprovals' }
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}
