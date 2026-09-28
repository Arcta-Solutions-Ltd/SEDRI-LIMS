using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Export Schedule list view.
/// Displays schedules for an export profile in the embedded list on the export profile record view.
/// </summary>
internal static class ExportScheduleListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Export Schedule list view.
    /// </summary>
    /// <returns>The list view configuration for the export schedules section.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"
        {
            'name': 'exportschedules',
            'type': 'ManageList',
            'title': '@ExpSch@',
            'headerText': '@ExpSch@.',
            'queryName': 'ExportScheduleListByProfileId',
            'parentId': 'ExportProfileId',
            'addButton': 'addexportscheduleuievent',
            'displaySummary': false,
            'DisplayIfEmpty': true,
            'gridColumns': [
                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'Name', 'minWidth': 150, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@ExpSchFreq@', 'fieldName': 'Frequency', 'minWidth': 100, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column3', 'name': '@ExpSchTime@', 'fieldName': 'TimeOfDay', 'minWidth': 100, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column4', 'name': '@ExpSchDay@', 'fieldName': 'DayOfMonth', 'minWidth': 80, 'maxWidth': 100, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column5', 'name': '@GenEna@', 'fieldName': 'Enabled', 'minWidth': 80, 'maxWidth': 100, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column6', 'name': '@ExpSchLast@', 'fieldName': 'LastRunAt', 'minWidth': 150, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': [
                { 'key': 'edit', 'text': '@GenEdiB@', 'icon': 'Edit', 'uievent': 'editexportscheduleuievent', 'onFinish': 'refresh', 'onSelect': true, 'primaryAction': 1 },
                { 'key': 'delete', 'text': '@GenDelC@', 'icon': 'Delete', 'uievent': 'deleteexportscheduleuievent', 'onFinish': 'refresh', 'onSelect': true, 'primaryAction': 2 }
            ]
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
