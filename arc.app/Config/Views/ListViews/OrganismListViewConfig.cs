using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class OrganismListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'organism',
                            'type': 'ManageList',
                            'title': '@OrgManA@',
                            'headerText': '@OrgManB@.',
                            singleQuery: 'singleorganismfororganismlist',
                            'queryName': 'OrganismList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenDes@', 'fieldName': 'Description', 'minWidth': 380, 'maxWidth': 380, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenCodA@', 'fieldName': 'Code', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true },
                                { 'key': 'column4', 'name': '@GenCus@', 'fieldName': 'Custom', 'minWidth': 80, 'maxWidth': 80, 'isResizable': true },
                                { 'key': 'column5', 'name': '@GenOrd@', 'fieldName': 'OrderName', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },
                                { 'key': 'column6', 'name': '@GenFam@', 'fieldName': 'FamilyName', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },                                
                                { 'key': 'column7', 'name': '@OrgPre@', 'fieldName': 'PreferredName', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },
                                { 'key': 'column8', 'name': '@OrgSys@', 'fieldName': 'Synonyms', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },

                            ],
                            'buttons': [
                                { key: 'lists', text: '@GenOrgF@', icon: 'List',
                                    buttons: [{key: 'addList', text: '@CodAddD@', icon: 'AddToShoppingList', uievent: 'addlistuievent', onFinish: 'refreshfilter' },
                                              {key: 'deleteList', text: '@CodDelD@', icon: 'RemoveFromShoppingList', uievent: 'deletelistuievent', onFinish: 'refreshfilter' }]
                                },
                                { key: 'addEntry', 'text': '@GenAddE@', 'icon': 'Add', uievent: 'addorganismuievent', onFinish: 'refresh', 
                                  rules : [{ effect: 'visible', field: 'codingid', rule: '!=', value: '676'}] },
                                { key: 'addCustomEntry', text: '@GenAddF@', icon: 'Add', uievent: 'addcustomuievent', onFinish: 'refresh',
                                  rules : [{ effect: 'visible', field: 'codingid', rule: '!=', value: '676'}] },
                                { key: 'editCustomEntry', text: '@GenEdiA@', icon: 'Edit', uievent: 'editcustomuievent', onFinish: 'update', onSelect: true,
                                  rules : [{ effect: 'visible', field: 'codingid', rule: '!=', value: '676'}] },
                                { key: 'deleteEntry', text: '@GenDelB@', icon: 'Remove', uievent: 'deleteorganismuievent', onFinish: 'refresh', onSelect: true, 
                                  rules : [{ effect: 'visible', field: 'codingid', rule: '!=', value: '676'}] },
                                { key: 'synonym', 'text': '@OrgSys@', 'icon': 'FileCSS', uievent: 'synonymuievent', onSelect: true }
                            ],
                            filters: [
                                { key: 'coding', placeholder: '@GenOrgE@', fieldName: 'codingid', multiSelect: false, width: 160, optionsName: 'Coding', dynamic: true, includeFixed: true, dropdownwidth: 850 }
                            ],
                            filterPresets: [
                                { key: 'filter1', name: '@OrgAll@', default: true, fields: [ { key: 'coding', values: [ '676'] } ] }
                            ],
                            filterSearch: true,
                            searchFields: [ 'searchText', 'Code' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}


                                //{ key: 'custom', text: '@GenCusA@', icon: 'CustomList', rules : [{ effect: 'visible', field: 'codingid', rule: '!=', value: '676'}],
                                //    buttons: [{key: 'addCustomEntry', text: '@GenAddE@', icon: 'Add', uievent: 'addcustomuievent', onFinish: 'refresh' },
                                //              {key: 'editCustomEntry', text: '@GenEdiA@', icon: 'Edit', uievent: 'editcustomuievent', onFinish: 'update', onSelect: true }]
                                //},
