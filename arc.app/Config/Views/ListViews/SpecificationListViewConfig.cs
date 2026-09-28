using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

internal class SpecificationListViewConfig
{
    internal static ListViewConfig GetView()
    {
        var view = @"{
                        'name': 'specifications',
                        'type': 'ManageList',
                        'title': '@GenSpfB@',
                        'headerText': '@GenSpfC@',
                        'queryName': 'SpecificationList',
                        'singleQuery': 'SingleSpecificationForSpecificationList',
                        'gridColumns': [
                            { 'key': 'column1', 'name': '@GenGui@', 'fieldName': 'Guidelines', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'column2', 'name': '@GenDoc@', 'fieldName': 'Document', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'column3', 'name': '@Ver@', 'fieldName': 'VersionNumber', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'column4', 'name': '@GenYeaB@', 'fieldName': 'PublicationYear', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                        ],
                        'buttons': [
                            { key: 'listsSource', text: '@TesGui@', icon: 'List',
                                buttons: [{key: 'addSource', text: '@CodAddI@', icon: 'AddToShoppingList', uievent: 'addsourceuievent', onFinish: 'refreshfilter' },
                                          {key: 'deleteSource', text: '@CodDelH@', icon: 'RemoveFromShoppingList', uievent: 'deletesourceuievent', onFinish: 'refreshfilter' }]
                            },
                            { key: 'listsDocument', text: '@GenDoc@', icon: 'List',
                                buttons: [{key: 'addDocument', text: '@SpfAddDocB@', icon: 'AddToShoppingList', uievent: 'adddocumentuievent', onFinish: 'refreshfilter' },
                                          {key: 'deleteDocument', text: '@SpfDelDocB@', icon: 'RemoveFromShoppingList', uievent: 'deletedocumentuievent', onFinish: 'refreshfilter' }]
                            },
                            { key: 'listsVersion', text: '@Ver@', icon: 'List',
                                buttons: [{key: 'addVersion', text: '@SpfAddVerB@', icon: 'AddToShoppingList', uievent: 'addversionuievent', onFinish: 'refreshfilter' },
                                          {key: 'deleteVersion', text: '@SpfDelVerB@', icon: 'RemoveFromShoppingList', uievent: 'deleteversionuievent', onFinish: 'refreshfilter' }]
                            },
                            { key: 'listsYear', text: '@GenYeaB@', icon: 'List',
                                buttons: [{key: 'addYear', text: '@SpfAddYeaB@', icon: 'AddToShoppingList', uievent: 'addyearuievent', onFinish: 'refreshfilter' },
                                          {key: 'deleteYear', text: '@SpfDelYeaB@', icon: 'RemoveFromShoppingList', uievent: 'deleteyearuievent', onFinish: 'refreshfilter' }]
                            },
                            { 'key': 'addspecification', 'text': '@SpfAddB@', 'icon': 'Add', 'onSelect': false, uievent: 'addspecificationuievent', onFinish: 'refresh' },
                            { 'key': 'editspecification', 'text': '@SpfEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editspecificationuievent', onFinish: 'update' },
                            { 'key': 'deletespecification', 'text': '@SpfDelB@', 'icon': 'Delete', 'onSelect': true, primaryAction: 2, uievent: 'deletespecificationuievent', onFinish: 'refresh' }
                        ],
                        filters: [
                            { key: 'Guidelines', placeholder: '@TesGui@', multiSelect: true, width: 140, optionsName: 'guidelines', fieldName: 'guidelinesid', dynamic: true, includeFixed: true },
                            { key: 'Document', placeholder: '@GenDoc@', multiSelect: true, width: 140, optionsName: 'documenttype', fieldName: 'documentid', dynamic: true, includeFixed: true },
                            { key: 'Version', placeholder: '@Ver@', multiSelect: true, width: 140, optionsName: 'version', fieldName: 'versionnumberid', dynamic: true, includeFixed: true },
                            { key: 'Year', placeholder: '@GenYeaB@', multiSelect: true, width: 140, optionsName: 'publicationyear', fieldName: 'publicationyearid', dynamic: true, includeFixed: true }
                        ],
                        filterSearch: true,
                        searchFields: [ 'searchText' ]
                    }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
