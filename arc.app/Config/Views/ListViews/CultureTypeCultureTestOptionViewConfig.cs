using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Culture Type Culture Test Option" view.
/// </summary>
internal class CultureTypeCultureTestOptionViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Culture Type Culture Test Option" view.
    /// </summary>
    /// <remarks>
    /// This configuration defines the list view's name, type, title, header text, query name, and add button.
    /// It includes settings for displaying the view when empty, defining grid columns, and configuring buttons 
    /// for actions such as editing and deleting culture type culture test options.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the list view configuration, including metadata 
    /// and settings such as grid columns, buttons, and query details.
    /// </returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'culturetypeculturetestoptionview',
                            'type': 'ManageList',
                            'title': '@LabCulA@',
                            'headerText': '@LabManD@.',
                            'queryName': 'culturetypeculturetestoptionlistquery',
                            'displaySummary': false,
                            'addbutton': 'addculturetypeculturetestoptionuievent',
                            'addButtonTooltip': '@LabAddI@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@CulTyp@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column2', 'name': '@ConCulB@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'edit', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editculturetypeculturetestoptionuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'delete', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deleteculturetypeculturetestoptionuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}