using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Expert Rule Condition list view.
/// Displays conditions for the selected expert rule in the record view.
/// </summary>
internal static class ExpertRuleConditionListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Expert Rule Condition list view.
    /// </summary>
    /// <returns>The list view configuration for the expert rule conditions section.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
            'name': 'expertruleconditions',
            'type': 'ManageList',
            'title': '@GenRulI@',
            'headerText': '@GenRulI@.',
            'queryName': 'ExpertRuleConditionListByExpertRuleId',
            'parentId': 'ExpertRuleId',
            'addButton': 'addexpertruleconditionuievent',
            'displaySummary': false,
            'DisplayIfEmpty': true,
            'gridColumns': [
                { 'key': 'column1', 'name': '@GenAnt@', 'fieldName': 'AntibioticDisplay', 'minWidth': 150, 'maxWidth': 250, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@BreTes@', 'fieldName': 'TestMethodName', 'minWidth': 120, 'maxWidth': 180, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column3', 'name': '@GenSus@', 'fieldName': 'SusceptibilityName', 'minWidth': 120, 'maxWidth': 180, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column4', 'name': '@BreSpe@', 'fieldName': 'SpecialConsiderationName', 'minWidth': 120, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column5', 'name': '@BreSta@', 'fieldName': 'StartVal', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column6', 'name': '@BreEnd@', 'fieldName': 'EndVal', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': [
                { key: 'edit', text: '@GenEdiB@', icon: 'Edit', onSelect: true, primaryAction: 1, uievent: 'editexpertruleconditionuievent', onFinish: 'embeddedrefresh' },
                { key: 'delete', text: '@GenDelC@', icon: 'Delete', onSelect: true, primaryAction: 2, uievent: 'deleteexpertruleconditionuievent', onFinish: 'embeddedrefresh' }
            ]
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
