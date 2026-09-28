using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Alert Test Criteria list view.
/// Displays test criteria for the selected alert.
/// </summary>
internal static class AlertTestCriteriaListViewConfig
{
    internal static ListViewConfig GetView()
    {
        var view = @"{
            'name': 'alerttestcriteria',
            'type': 'ManageList',
            'title': '@AleTes@',
            'headerText': '@AleTes@.',
            'queryName': 'AlertTestCriteriaListByAlertId',
            'parentId': 'AlertId',
            'gridColumns': [
                { 'key': 'column1', 'name': '@AleTesB@', 'fieldName': 'TestName', 'minWidth': 150, 'maxWidth': 250, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@GenFieA@', 'fieldName': 'FieldName', 'minWidth': 120, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column3', 'name': '@AleCom@', 'fieldName': 'Comparison', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column4', 'name': '@GenVal@', 'fieldName': 'CompValue', 'minWidth': 100, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': []
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
