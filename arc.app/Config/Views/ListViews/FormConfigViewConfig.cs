using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class FormConfigViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'formconfig',
                            'type': 'ManageList',
                            'title': '@GenFor@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'formconfiglistquery',
                            'displaySummary': false,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenForA@', 'fieldName': 'Description', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@GenEve@', 'fieldName': 'Event', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'view', 'text': '@GenVieC@', 'icon': 'RedEye', 'onSelect': true, primaryAction: 1, uievent: 'pageconfiguievent' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
