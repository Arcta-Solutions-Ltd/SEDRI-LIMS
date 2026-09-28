using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Specimen Type Direct Test Mapping" view.
/// </summary>
internal class SpecimenTypeDirectTestMappingViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Specimen Type Direct Test Mapping" view.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the list view's name, type, title, header text, query name, and add button.
    /// It includes settings for displaying the view when empty, defining grid columns, and configuring buttons 
    /// for actions such as editing and deleting specimen type direct test mappings.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the list view configuration, including metadata 
    /// and settings such as grid columns, buttons, and query details.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'specimentypedirecttest',
                            'type': 'ManageList',
                            'title': '@ConSpeA@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'specimentypedirecttestmappinglistquery',
                            'displaySummary': false,
                            'addbutton': 'addspecimentypedirecttestuievent',
                            'addButtonTooltip': '@ConAddE@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@SpeSpeB@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ConDir@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'editspecimentypeculturetype', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editspecimentypedirecttestuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deletespecimentypeculturetype', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deletespecimentypedirecttestuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
