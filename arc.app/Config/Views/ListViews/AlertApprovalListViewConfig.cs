using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Alert Approval list view.
/// Displays approval/rejection history for the selected alert.
/// </summary>
internal static class AlertApprovalListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Alert Approval list view.
    /// </summary>
    /// <returns>The list view configuration for the alert approvals section.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"
        {
            'name': 'alertapprovals',
            'type': 'ManageList',
            'title': '@BreAppHis@',
            'headerText': '@BreAppHis@.',
            'queryName': 'AlertApprovalListByAlertId',
            'parentId': 'AlertId',
            'addButton': 'addalertapprovaluievent',
            'displaySummary': false,
            'DisplayIfEmpty': true,
            'gridColumns': [
                { 'key': 'column1', 'name': '@GenStaA@', 'fieldName': 'codingstatus', 'minWidth': 120, 'maxWidth': 160, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@BreDatRec@', 'fieldName': 'daterecorded', 'minWidth': 150, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column3', 'name': '@BreRecBy@', 'fieldName': 'recordedby', 'minWidth': 120, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': []
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
