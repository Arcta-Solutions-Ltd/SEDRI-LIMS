using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the Alert record view.
/// Displays alert details at the top when View is clicked from the alert list view,
/// with susceptibility criteria and test criteria list view sections.
/// </summary>
internal class AlertRecordViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the Alert record view.
    /// </summary>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@AleMan@',
                        'name': 'alerts',
                        'type': 'recordview',
                        'refreshRecordOnEmbeddedSave': true,
                        buttons: [],
                        'regions': [
                            { 'id': 'alertdetails', 'type': 'standard', 'queryName': 'alertviewquery' },
                            { 'id': 'alertsusceptibilitycriteria', 'type': 'listview', 'title': '@AleSus@', 'listViewName': 'alertsusceptibilitycriteria' },
                            { 'id': 'alerttestcriteria', 'type': 'listview', 'title': '@AleTes@', 'listViewName': 'alerttestcriteria' },
                            { 'id': 'alertapprovals', 'type': 'listview', 'title': '@BreAppHis@', 'listViewName': 'alertapprovals' }
                        ]
                    }";

        return JsonConvert.DeserializeObject<RecordViewConfig>(view);
    }
}
