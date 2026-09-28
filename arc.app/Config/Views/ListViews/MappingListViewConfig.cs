using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;
/// <summary>
/// Represents the configuration for the Mapping list view.
/// </summary>
internal class MappingListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Mapping list view.
    /// </summary>
    /// <returns>The list view configuration for the Mapping list view.</returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'mappingconfigview',
                            'type': 'ManageList',
                            'title': '@MapTit@',
                            'headerText': '@MapHea@',
                            'queryName': 'mappinglistquery',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'Name', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            ],
                            'buttons': [
                                { key: 'addmapping', 'text': '@MapAdd@', 'icon': 'Add', uievent: 'addmappinguievent', onFinish: 'refresh' },
                                { key: 'editmappin', text: '@MapEdi@', icon: 'Edit', uievent: 'editmappinguievent', onFinish: 'refresh', onSelect: true, primaryAction: 1 },
                                { key: 'deletemapping', text: '@MapDel@', icon: 'Delete', uievent: 'deletemappinguievent', onFinish: 'refresh', onSelect: true, primaryAction: 2 }
                            ],
                            searchFields: [ 'key', 'Value' ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
