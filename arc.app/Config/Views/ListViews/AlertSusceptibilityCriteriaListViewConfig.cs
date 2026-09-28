using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Alert Susceptibility Criteria list view.
/// Displays antibiotic and susceptibility criteria for the selected alert.
/// </summary>
internal static class AlertSusceptibilityCriteriaListViewConfig
{
    internal static ListViewConfig GetView()
    {
        var view = @"{
            'name': 'alertsusceptibilitycriteria',
            'type': 'ManageList',
            'title': '@AleSus@',
            'headerText': '@AleSus@.',
            'queryName': 'AlertSusceptibilityCriteriaListByAlertId',
            'parentId': 'AlertId',
            'gridColumns': [
                { 'key': 'column1', 'name': '@GenAnt@', 'fieldName': 'AntibioticNames', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@GenSus@', 'fieldName': 'SusceptibilityName', 'minWidth': 120, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': []
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
