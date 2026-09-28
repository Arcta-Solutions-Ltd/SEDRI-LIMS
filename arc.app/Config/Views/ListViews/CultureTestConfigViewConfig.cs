using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Culture Test Configuration list view.
    /// </summary>
    internal class CultureTestConfigViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Culture Test Configuration list view.
        /// </summary>
        /// <returns>The list view configuration for the Culture Test Configuration list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'culturetestconfig',
                            'type': 'ManageList',
                            'title': '@SpeMan@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'culturetestconfiglistquery',
                            'displaySummary': false,
                            'addbutton': 'addculturetestuievent',
                            'addButtonTooltip': '@ConAddA@',
                            'gridColumns': [
                                { 'key': 'column2', 'name': '@GenDes@', 'fieldName': 'Description', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'view', 'text': '@SpeVie@', 'icon': 'RedEye', 'onSelect': true, primaryAction: 1, uievent: 'pageconfiguievent' },
                                { 'key': 'editculturetest', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 2, uievent: 'editculturetestuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deleteculturetest', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 3, uievent: 'deleteculturetestuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'disableculturetest', 'text': '@GenDisB@', 'icon': 'DisableUpdates', 'onSelect': true, primaryAction: 4, uievent: 'disableculturetestuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
