using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Expert Rule Action list view.
/// Displays actions for the selected expert rule in the record view.
/// </summary>
internal static class ExpertRuleActionListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Expert Rule Action list view.
    /// </summary>
    /// <returns>The list view configuration for the expert rule actions section.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
            'name': 'expertruleactions',
            'type': 'ManageList',
            'title': '@GenRulF@',
            'headerText': '@GenRulF@.',
            'queryName': 'ExpertRuleActionListByExpertRuleId',
            'parentId': 'ExpertRuleId',
            'addButton': 'addexpertruleactionuievent',
            'displaySummary': false,
            'DisplayIfEmpty': true,
            'gridColumns': [
                { 'key': 'column1', 'name': '@GenAnt@', 'fieldName': 'AntibioticDisplay', 'minWidth': 150, 'maxWidth': 250, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column1b', 'name': '@InsAnt@', 'fieldName': 'AntibioticGroupDisplay', 'minWidth': 150, 'maxWidth': 250, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@GenSus@', 'fieldName': 'SusceptibilityName', 'minWidth': 120, 'maxWidth': 180, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column3', 'name': '@GenInc@', 'fieldName': 'DisplayOnReport', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': [
                { key: 'edit', text: '@GenEdiB@', icon: 'Edit', onSelect: true, primaryAction: 1, uievent: 'editexpertruleactionuievent', onFinish: 'embeddedrefresh' },
                { key: 'delete', text: '@GenDelC@', icon: 'Delete', onSelect: true, primaryAction: 2, uievent: 'deleteexpertruleactionuievent', onFinish: 'embeddedrefresh' }
            ]
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
