using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Report List View."
/// </summary>
internal class ReportListViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Report List View."
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the "Report List View."
    /// It includes metadata such as name, type, title, header text, query name, and display settings. 
    /// Grid columns specify attributes like name and enabled status, while buttons provide actions such as 
    /// viewing, editing, or deleting report configurations.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the report list view configuration,
    /// including grid columns and associated buttons.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'reportlistconfig',
                            'type': 'ManageList',
                            'title': '@GenVieD@',
                            'headerText': '@GenVieD@.',
                            'queryName': 'reportconfiglistquery',
                            'displaySummary': false,
                            'addbutton': 'addreportconfiguievent',
                            'addButtonTooltip': '@ConAddL@',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'Name', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', name: '@GenEna@', fieldName: 'Enabled', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'view', 'text': '@GenVieC@', 'icon': 'RedEye', 'onSelect': true, primaryAction: 1, uievent: 'reportconfiguievent' },
                                { 'key': 'editreportconfig', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 2, uievent: 'editreportconfiguievent', onFinish: 'embeddedrefresh', entrystates: 'canconfig' },
                                { 'key': 'deletereportconfig', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 3, uievent: 'deletereportconfiguievent', onFinish: 'embeddedrefresh', entrystates: 'canconfig' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}