using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Accession Number list view.
    /// </summary>
    internal class AccessionNumberListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Accession Number view.
        /// </summary>
        /// <returns>The list view configuration for the Accession Number view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'accessionnumberlistview',
                            'type': 'ManageList',
                            'title': '@TesAddB@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'accessionnumberquery',
                            'addButton': 'addaccessionnumbertextuievent',
                            'editButton': 'editaccessionnumberuievent',
                            'displaySummary': false,
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenDes@', 'fieldName': 'Name', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@GenEna@', 'fieldName': 'Enabled', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: false },
                                { 'key': 'column3', 'name': '@GenResC@', 'fieldName': 'Enabled2', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'editsetting', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editsettinguievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deletesetting', 'text': '@GenDelC@', 'icon': 'Trash', 'onSelect': true, primaryAction: 2, uievent: 'deletesettinguievent', onFinish: 'embeddedrefresh', entryStates: 'AllowDelete'},
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
