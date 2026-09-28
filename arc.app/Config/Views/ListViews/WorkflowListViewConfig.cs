using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Workflow List View."
/// </summary>
internal class WorkflowListViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Workflow List View."
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the "Workflow List View." 
    /// It includes metadata such as name, type, title, header text, query name, grid columns, and associated buttons. 
    /// Grid columns specify the attributes to display, while buttons provide actions like adding, editing, or deleting workflows.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the workflow list view configuration, including grid columns and button actions.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'workflows',
                            'type': 'ManageList',
                            'title': '@GenWor@',
                            'headerText': '@ConCreA@',
                            'queryName': 'WorkflowListQuery',
                            'addButton': 'addworkflowuievent',
                            'addButtonTooltip': '@ConAddAB@',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'Name', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ExpProD@', 'fieldName': 'Description', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            ],
                            'buttons': [
                                { 'key': 'view', 'text': '@GenVieC@', 'icon': 'RedEye', 'onSelect': true, primaryAction: 1, uievent: 'viewworkflowuievent' },
                                { 'key': 'editworkflow', 'text': '@ConEdiAC@', 'icon': 'Edit', 'uievent':'editworkflowuievent', primaryAction: 3, onSelect: true },
                                { 'key': 'deletelaboratory', 'text': 'Delete Workflow', 'icon': 'Delete', 'uievent':'deleteworkflowuievent', primaryAction: 3, onSelect: true }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
