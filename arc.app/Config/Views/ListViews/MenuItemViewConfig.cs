using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class MenuItemViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'menuitemconfig',
                            'type': 'ManageList',
                            'title': '@SpeMan@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'menuitemconfiglistquery',
                            'displaySummary': false,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenTex@', 'fieldName': 'Text', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@GenFor@', 'fieldName': 'Form', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false },
                                { 'key': 'column3', 'name': '@ConPar@', 'fieldName': 'ParentButton', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [

                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
