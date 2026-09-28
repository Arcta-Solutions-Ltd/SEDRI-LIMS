using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Test Categorisation" view.
/// </summary>
internal class TestCategorisationViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Test Categorisation" view.
    /// </summary>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the configuration for managing test categorisation.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                        'name': 'testcategorisationview',
                        'type': 'ManageList',
                        'title': '@LabTesB@',
                        'headerText': '@LabManB@.',
                        'queryName': 'testcategorylistquery',
                        'displaySummary': false,
                        'addbutton': 'addtestcategoryuievent',
                        'addButtonTooltip': '@LabAddA@',
                        'DisplayIfEmpty':true,
                        'gridColumns': [
                            { 'key': 'column1', 'name': '@GenCat@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                            { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: false },
                            { 'key': 'column2', 'name': '@ConDir@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                        ],
                        'buttons': [
                            { 'key': 'edit', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'edittestcategoryuievent', onFinish: 'embeddedrefresh' },
                            { 'key': 'delete', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deletetestcategoryuievent', onFinish: 'embeddedrefresh' }
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
