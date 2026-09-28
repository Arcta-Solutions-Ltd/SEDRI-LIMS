using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class GeneralSettingsListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'generalsettingslistview',
                            'type': 'ManageList',
                            'title': '@TesAddB@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'generalsettingsquery',
                            'displaySummary': false,
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenDes@', 'fieldName': 'Name', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false }
                            ],
                            'buttons': [
                                 { 'key': 'editsetting', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editsettinguievent', onFinish: 'embeddedrefresh' },
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
