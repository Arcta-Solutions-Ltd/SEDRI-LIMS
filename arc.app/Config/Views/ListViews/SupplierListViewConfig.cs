using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;
/// <summary>
/// Represents the configuration for the Supplier List View.
/// </summary>
internal class SupplierListViewConfig
{
    /// <summary>
    /// Generates and returns the configuration for the Supplier List View.
    /// </summary>
    /// <returns>A <see cref="ListViewConfig"/> object representing the view configuration.</returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'suppliers',
                            'type': 'ManageList',
                            'title': '@SupMan@',
                            'headerText': '@SupCre@.',
                            'queryName': 'SupplierListQuery',
                            'singleQuery': 'SingleSupplierForSupplierListQuery',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@SupSup@', 'fieldName': 'name', 'minWidth': 100, 'maxWidth': 300, 'isResizable': true },
                                { 'key': 'column2', 'name': '@GenCodA@', 'fieldName': 'code', 'minWidth': 100, 'maxWidth': 300, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenStaA@', 'fieldName': 'supplierstatus', 'minWidth': 100, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column4', name: '@GenLoc@', fieldName: 'fullyqualifiedname', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true }
                            ],
                            'buttons': [
                                { 'key': 'addsupplier', 'text': '@SupAdd@', 'icon': 'Add', 'onSelect': false, uievent: 'addsupplieruievent', onFinish: 'refresh' },
                                { 'key': 'editsupplier', 'text': '@SupEdi@', 'icon': 'Edit', 'onSelect': true, uievent: 'editsupplieruievent', onFinish: 'update' },
                                { 'key': 'deletesupplier', 'text': '@SupDel@', 'icon': 'Delete', 'onSelect': true, uievent: 'deletesupplieruievent', onFinish: 'refresh' }
                            ],
                            filters: [
                                { key: 'location', placeholder: '@GenLoc@', width: 160, optionsName: 'LocationList', fieldName: 'LocationId', type: 'picker' },
                                { key: 'supplierstatus', placeholder: '@GenStaA@', multiSelect: true, width: 240, optionsName: 'supplierstatus', fieldName: 'supplierstatusid' }
                            ],
                            filterSearch: true,
                            searchFields: [ 'name','code' ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);
        return result;
    }
}

