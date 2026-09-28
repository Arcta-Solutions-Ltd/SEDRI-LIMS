using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Culture Test Culture Type list view.
    /// </summary>
    internal class CultureTypeCultureTestMappingViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Culture Test Culture Type list view.
        /// </summary>
        /// <returns>The list view configuration for the Culture Test Culture Type list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'culturetypeculturetest',
                            'type': 'ManageList',
                            'title': '@ConSpeB@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'culturetypeculturetestmappinglistquery',
                            'displaySummary': false,
                            'addbutton': 'addculturetypeculturetestuievent',
                            'addButtonTooltip': '@ConAddZ@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@CulTyp@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ConCulB@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'editculturetypeculturetest', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editculturetypeculturetestuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deleteculturetypeculturetest', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deleteculturetypeculturetestuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
