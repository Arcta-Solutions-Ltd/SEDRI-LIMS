using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Configuration History list view.
    /// </summary>
    internal class ConfigHistoryListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Configuration History list view.
        /// </summary>
        /// <returns>The list view configuration for the Configuration History list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view =
                @"{
                    'name': 'confighistory',
                    'type': 'ManageList',
                    'title': '@ConConA@',
                    'headerText': '@ConConB@.',
                    'queryName': 'confighistorylistquery',
                    'gridColumns': [
                        { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'configname', 'minWidth': 380, 'maxWidth': 380, 'isResizable': true },
                        { key: 'column2', name: '@SpeRecD@', fieldName: 'lastmodifieddate', minWidth: 110, maxWidth: 110, isResizable: true, isCollapsible: true }
                    ],
                    'buttons': [
                                { 'key': 'exportconfig', 'text': '@ConExp@', 'icon': 'Add', 'uievent': 'exportconfigurationuievent' },
                                { 'key': 'importconfig', 'text': '@ConImp@', 'icon': 'Add', 'uievent': 'importconfigurationuievent' }      
                    ],
                    'filterSearch': true,
                    'searchFields': [ 'configname' ]
                }";

            return JsonConvert.DeserializeObject<ListViewConfig>(view);
        }
    }
}
