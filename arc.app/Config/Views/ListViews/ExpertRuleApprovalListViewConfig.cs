using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Expert Rule Approval list view.
/// Displays approval/rejection history for the selected expert rule.
/// </summary>
internal static class ExpertRuleApprovalListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Expert Rule Approval list view.
    /// </summary>
    /// <returns>The list view configuration for the expert rule approvals section.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"
        {
            'name': 'expertruleapprovals',
            'type': 'ManageList',
            'title': '@BreAppHis@',
            'headerText': '@BreAppHis@.',
            'queryName': 'ExpertRuleApprovalListByExpertRuleId',
            'parentId': 'ExpertRuleId',
            'addButton': 'addexpertruleapprovaluievent',
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
