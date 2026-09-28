using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Specimen Type Culture Type Option" view.
/// </summary>
internal class SpecimenTypeCultureTypeOptionViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Specimen Type Culture Type Option" view.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the list view's name, type, title, header text, query name, and add button.
    /// It includes settings for displaying the view when empty, defining grid columns, and configuring buttons 
    /// for actions such as editing and deleting specimen type culture type options.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the list view configuration, including metadata 
    /// and settings such as grid columns, buttons, and query details.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'specimentypeculturetypeoptionview',
                            'type': 'ManageList',
                            'title': '@LabCulA@',
                            'headerText': '@LabManD@.',
                            'queryName': 'specimentypeculturetypeoptionlistquery',
                            'displaySummary': false,
                            'addbutton': 'addspecimentypeculturetypeoptionuievent',
                            'addButtonTooltip': '@LabAddH@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@SpeSpeB@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ConCul@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'edit', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editspecimentypeculturetypeoptionuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'delete', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deletespecimentypeculturetypeoptionuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
