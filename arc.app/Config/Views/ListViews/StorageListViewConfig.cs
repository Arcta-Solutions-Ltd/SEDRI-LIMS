using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration class for defining the view of the storage list management interface.
/// </summary>
internal class StorageListViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the storage list view.
    /// </summary>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object that defines the storage list view, 
    /// including grid columns, buttons, filters, and search fields.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                        'name': 'storage',
                        'type': 'ManageList',
                        'title': '@SupManA@',
                        'headerText': '@SupCreA@.',
                        'queryName': 'StorageListQuery',
                        'singleQuery': 'SingleStorageForStorageListQuery',
                        'gridColumns': [
                            { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'storagename', 'minWidth': 100, 'maxWidth': 200, 'isResizable': true },
                            { 'key': 'column2', 'name': '@GenCodA@', 'fieldName': 'code', 'minWidth': 50, 'maxWidth': 100, 'isResizable': true },
                            { 'key': 'column3', 'name': '@GenHie@', 'fieldName': 'fullyqualifiedname', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'column4', 'name': '@SupStoA@', 'fieldName': 'storagetype', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'column5', 'name': '@GenEna@', 'fieldName': 'enabled', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                        ],
                        'buttons': [
                            { 'key': 'addstorage', 'text': '@SupAddB@', 'icon': 'Add', 'onSelect': false, uievent: 'addstorageuievent', onFinish: 'refresh' },
                            { 'key': 'editstorage', 'text': '@SupEdiC@', 'icon': 'Edit', 'onSelect': true, uievent: 'editstorageuievent', onFinish: 'update' },
                            { 'key': 'deletestorage', 'text': '@SupDelC@', 'icon': 'Delete', 'onSelect': true, uievent: 'deletestorageuievent', onFinish: 'refresh' }
                        ],
                        filters: [
                            { key: 'storagetype', placeholder: '@SupStoA@', multiSelect: true, width: 240, optionsName: 'storagetype', fieldName: 'storagetypeid' }
                        ],
                        filterSearch: true,
                        searchFields: [ 'storagename', 'code' ]
                    }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}

