using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Specimen Type Culture Type Mapping" view.
/// </summary>
internal class SpecimenTypeCultureTypeMappingViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Specimen Type Culture Type Mapping" view.
    /// </summary>
    /// <remarks>
    /// This configuration defines the list view's name, type, title, header text, query name, and add button.
    /// It includes settings for displaying the view when empty, defining grid columns, and configuring buttons 
    /// for actions such as editing and deleting specimen type culture type mappings.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the list view configuration, including metadata 
    /// and settings such as grid columns, buttons, and query details.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'specimentypeculturetype',
                            'type': 'ManageList',
                            'title': '@ConSpe@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'specimentypeculturetypemappinglistquery',
                            'displaySummary': false,
                            'addbutton': 'addspecimentypeculturetypeuievent',
                            'addButtonTooltip': '@ConAddD@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@SpeSpeB@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ConCul@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'editspecimentypeculturetype', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editspecimentypeculturetypeuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deletespecimentypeculturetype', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deletespecimentypeculturetypeuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
