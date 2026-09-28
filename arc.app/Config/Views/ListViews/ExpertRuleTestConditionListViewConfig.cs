using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Expert Rule Test Condition list view.
/// Displays test conditions for the selected expert rule in the record view.
/// </summary>
internal static class ExpertRuleTestConditionListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Expert Rule Test Condition list view.
    /// </summary>
    /// <returns>The list view configuration for the expert rule test conditions section.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
            'name': 'expertruletestconditions',
            'type': 'ManageList',
            'title': '@RulTes@',
            'headerText': '@RulTes@.',
            'queryName': 'ExpertRuleTestConditionListByExpertRuleId',
            'parentId': 'ExpertRuleId',
            'addButton': 'addexpertruletestconditionuievent',
            'displaySummary': false,
            'DisplayIfEmpty': true,
            'gridColumns': [
                { 'key': 'column1', 'name': '@AleTesB@', 'fieldName': 'TestName', 'minWidth': 150, 'maxWidth': 250, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@GenFieA@', 'fieldName': 'FieldName', 'minWidth': 120, 'maxWidth': 180, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column3', 'name': '@AleCom@', 'fieldName': 'Comparison', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column4', 'name': '@GenVal@', 'fieldName': 'CompValue', 'minWidth': 100, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': [
                { key: 'edit', text: '@GenEdiB@', icon: 'Edit', onSelect: true, primaryAction: 1, uievent: 'editexpertruletestconditionuievent', onFinish: 'embeddedrefresh' },
                { key: 'delete', text: '@GenDelC@', icon: 'Delete', onSelect: true, primaryAction: 2, uievent: 'deleteexpertruletestconditionuievent', onFinish: 'embeddedrefresh' }
            ]
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
