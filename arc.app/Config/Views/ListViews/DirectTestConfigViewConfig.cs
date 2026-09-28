using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Direct Test Configuration list view.
    /// </summary>
    internal class DirectTestConfigViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Direct Test Configuration list view.
        /// </summary>
        /// <returns>The list view configuration for the Direct Test Configuration list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'directtestconfig',
                            'type': 'ManageList',
                            'title': '@SpeMan@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'directtestconfiglistquery',
                            'displaySummary': false,
                            'addbutton': 'adddirecttestuievent',
                            'addButtonTooltip': '@ConAddB@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column2', 'name': '@GenDes@', 'fieldName': 'Description', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'view', 'text': '@GenVieC@', 'icon': 'RedEye', 'onSelect': true, primaryAction: 1, uievent: 'pageconfiguievent' },
                                { 'key': 'editdirecttest', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 2, uievent: 'editdirecttestuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deletedirecttest', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 3, uievent: 'deletedirecttestuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'disabledirecttest', 'text': '@GenDisB@', 'icon': 'DisableUpdates', 'onSelect': true, primaryAction: 4, uievent: 'disabledirecttestuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
