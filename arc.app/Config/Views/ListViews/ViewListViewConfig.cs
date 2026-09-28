using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class ViewListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'views',
                            'type': 'ManageList',
                            'title': '@VieMan@',
                            'headerText': '@VieTxt@',
                            'queryName': 'ViewList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenVieE@', 'fieldName': 'Name', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true },
                                { 'key': 'menu', name: '', 'fieldName': '', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column2', 'name': '@GenTit@', 'fieldName': 'Title', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true }
                            ],
                            'buttons': [
                                { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewconfiguievent' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
