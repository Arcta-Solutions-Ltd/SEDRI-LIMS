using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Defines the locations list view configured for hierarchy display with parentId: 'parentlocationid'.
    /// </summary>
    internal class LocationListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'locations',
                            'type': 'ManageList',
                            'parentId': 'parentlocationid',
                            'title': '@LocMan@',
                            'headerText': '@LocCre@.',
                            'queryName': 'LocationList',
                            'singleQuery': 'SingleLocationForLocationList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@LocLoc@', 'fieldName': 'name', 'minWidth': 100, 'maxWidth': 200, 'isResizable': true },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true },
                                { 'key': 'column2', 'name': '@GenCodA@', 'fieldName': 'code', 'minWidth': 50, 'maxWidth': 100, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenHie@', 'fieldName': 'fullyqualifiedname', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true, 'hideInHierarchy': true },
                                { 'key': 'column4', 'name': '@GenEna@', 'fieldName': 'enabled', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'addlocation', 'text': '@LocAdd@', 'icon': 'Add', 'onSelect': false, uievent: 'addlocationuievent', onFinish: 'refresh' },
                                { 'key': 'addchildlocation', 'text': '@LocAddChild@', 'icon': 'Add', 'onSelect': true, 'primaryAction': 1, uievent: 'addlocationuievent', onFinish: 'refresh', addChildContext: true, prefillFormFields: { 'ParentLocationId': 'id' } },
                                { 'key': 'editlocation', 'text': '@LocEdi@', 'icon': 'Edit', 'onSelect': true, 'primaryAction': 1, uievent: 'editlocationuievent', onFinish: 'update' },
                                { 'key': 'deletelocation', 'text': '@LocDel@', 'icon': 'Delete', 'onSelect': true, 'primaryAction': 2, uievent: 'deletelocationuievent', onFinish: 'refresh' }
                            ],
                            filterSearch: true,
                            searchFields: [ 'name', 'code', 'fullyqualifiedname' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
