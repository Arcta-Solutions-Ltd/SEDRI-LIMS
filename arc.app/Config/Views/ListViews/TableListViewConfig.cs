using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class TableListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'tables',
                            'type': 'ManageList',
                            'title': '@TabMan@',
                            'headerText': '@TabManA@',
                            'queryName': 'ListContents',
                            'singleQuery': 'singletableentryforlist',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenVal@', 'fieldName': 'Value', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column2', 'name': '@GenPar@', 'fieldName': 'Parent', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column2', 'name': '@GenEna@', 'fieldName': 'Enabled', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                            ],
                            'buttons': [
                                { key: 'tables', text: '@GenTab@', icon: 'Table',
                                    buttons: [{key: 'addTable', text: '@TabAddC@', icon: 'Add', uievent: 'addtableuievent', onFinish: 'refreshfilter' },
                                              {key: 'editTable', text: '@TabEdiD@', icon: 'Edit', uievent: 'edittableuievent', onFinish: 'refreshfilter' },
                                              {key: 'deleteTable', text: '@TabDelC@', icon: 'Remove', uievent: 'deletetableuievent', onFinish: 'refreshfilter' }]
                                },
                                { 'key': 'addtableentry', 'text': '@TabAddB@', 'icon': 'Add', 'onSelect': false, uievent: 'addtableentryuievent', onFinish: 'refresh' },
                                { 'key': 'edittableentry', 'text': '@TabEdiA@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'edittableentryuievent', onFinish: 'update' },
                                { 'key': 'deletetableentry', 'text': '@TabDelB@', 'icon': 'Delete', 'onSelect': true, primaryAction: 2, uievent: 'deletetableentryuievent', onFinish: 'refresh' },
                                { 'key': 'ordertable', 'text': '@TabOrd@', 'icon': 'ActivateOrders', 'onSelect': false, primaryAction: 3, uievent: 'ordertableuievent', onFinish: 'refresh' }
                            ],
                            filters: [
                                { key: 'commonlist', fieldName: 'listid', multiSelect: false, width: 160, optionsName: 'CommonList', dynamic: true }
                            ],
                            filterPresets: [
                                { key: 'filter1', name: '@SpeSpeB@', default: true, fields: [ { key: 'commonlist', values: [ '4'] } ] },
                                { key: 'filter2', name: '@SpeSpeC@', default: false, fields: [ { key: 'commonlist', values: [ '5'] } ] },
                                { key: 'filter3', name: '@SpeSpeF@', default: false, fields: [ { key: 'commonlist', values: [ '54'] } ] },
                                { key: 'filter4', name: '@SpeRecB@', default: false, fields: [ { key: 'commonlist', values: [ '6'] } ] }
                            ],
                            filterSearch: true,
                            searchFields: [ 'searchText' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
