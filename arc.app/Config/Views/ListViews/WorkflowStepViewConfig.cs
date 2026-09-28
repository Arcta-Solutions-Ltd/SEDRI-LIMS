using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Workflow Step View."
/// </summary>
internal class WorkflowStepViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Workflow Step View."
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the "Workflow Step View."
    /// It includes metadata such as name, type, title, header text, query name, display settings, and grid columns.
    /// Grid columns specify attributes like event name and entry states, while buttons provide actions such as 
    /// editing or deleting workflow entries.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the workflow step view configuration,
    /// including grid columns and associated buttons for managing workflow entries.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'workflowstep',
                            'type': 'ManageList',
                            'title': '@ConWor@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'workflowsteplistquery',
                            'displaySummary': false,
                            'addbutton': 'addworkflowentryuievent',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenEve@', 'fieldName': 'EventName', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ConEnt@', 'fieldName': 'EntryStates', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'editworkflowentry', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editworkflowentryuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deleteworkflowentry', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 2, uievent: 'deleteworkflowentryuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
