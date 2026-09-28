using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the Breakpoint Line list view.
/// Displays susceptibility, start value, and end value from resultline for the selected breakpoint.
/// </summary>
internal static class BreakpointLineListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Breakpoint Line list view.
    /// </summary>
    /// <returns>The list view configuration for the breakpoint lines section.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
            'name': 'breakpointlines',
            'type': 'ManageList',
            'title': '@BreLin@',
            'headerText': '@BreLin@.',
            'queryName': 'BreakpointLineListByBreakpointId',
            'parentId': 'BreakpointId',
            'gridColumns': [
                { 'key': 'column1', 'name': '@GenSus@', 'fieldName': 'SusceptibilityName', 'minWidth': 120, 'maxWidth': 180, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2', 'name': '@BreSta@', 'fieldName': 'StartVal', 'minWidth': 120, 'maxWidth': 160, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column3', 'name': '@BreEnd@', 'fieldName': 'EndVal', 'minWidth': 120, 'maxWidth': 160, 'isResizable': true, 'isCollapsible': false }
            ],
            'buttons': []
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
