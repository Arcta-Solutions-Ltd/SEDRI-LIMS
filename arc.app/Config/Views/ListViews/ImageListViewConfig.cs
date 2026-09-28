using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the image list view
/// </summary>
internal class ImageListViewConfig
{
    internal ListViewConfig GetView()
    {
        var view = @"{
            'name': 'images',
            'type': 'ManageList',
            'title': '@ImgMan@',
            'headerText': '@ImgCreA@.',
            'queryName': 'ImageListQuery',
            'singleQuery': 'singleimagequery',
            'multiselect': false,
            'gridColumns': [
                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'name', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': false },
                { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                { 'key': 'column2', 'name': '@GenDes@', 'fieldName': 'description', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, 'isCollapsible': true }
            ],
            'buttons': [
                { 'key': 'upload', 'text': '@ImgUpl@', 'icon': 'Upload', 'uievent': 'uploadimageuievent', 'onFinish': 'refresh' },
                { 'key': 'update', 'text': '@ImgEdi@', 'icon': 'Edit', 'onSelect': true, 'primaryAction': 1, 'uievent': 'updateimageuievent', 'onFinish': 'update' },
                { 'key': 'delete', 'text': '@ImgDel@', 'icon': 'Delete', 'onSelect': true, 'primaryAction': 2, 'uievent': 'deleteimageuievent', 'onFinish': 'refresh' }
            ],
            'filters': [
            ],
            'filterSearch': true,
            'searchFields': ['name', 'description']
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view)!;
    }
}
