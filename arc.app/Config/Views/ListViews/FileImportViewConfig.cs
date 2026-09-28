using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class FileImportViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'fileimport',
                            'type': 'ManageList',
                            'title': '@ImpPro@',
                            'headerText': '@ImpImpB@',
                            'queryName': 'ImportFileListQuery',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenFilC@', 'fieldName': 'filename', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true },
                                { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ImpProA@', 'fieldName': 'name', 'minWidth': 450, 'maxWidth': 450, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenStaA@', 'fieldName': 'importstatus', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'loadimportfile', 'text': '@ImpImpC@', 'icon': 'Add', uievent: 'loadimportfileuievent', onFinish: 'refresh' },
                                 { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewloadedfileinformationzuievent', onFinish: 'refresh'  }
                            ]
                            
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
