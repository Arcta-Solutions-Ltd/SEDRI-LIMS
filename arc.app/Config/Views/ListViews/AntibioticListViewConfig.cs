using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Antibiotics list view.
    /// </summary>
    internal class AntibioticListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Antibiotics list view.
        /// </summary>
        /// <returns>The list view configuration for the Antibiotics list view.</returns>
        internal ListViewConfig GetView()
        {
            var view =
                @"{
                    'name': 'antibiotics',
                    'type': 'ManageList',
                    'title': '@AntManA@',
                    'headerText': '@AntManB@.',
                    'queryName': 'antibioticlist',
                    'singleQuery': 'AntibioticEntryByIdQuery',
                    'gridColumns': [
                        { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'AntibioticName', 'minWidth': 380, 'maxWidth': 380, 'isResizable': true },
                        { 'key': 'column2', 'name': '@GenCod@', 'fieldName': 'Code', 'minWidth': 80, 'maxWidth': 80, 'isResizable': true },
                    ],
                    'buttons': [
                                { key: 'lists', text: '@AntAnt@', icon: 'List',
                                    buttons: [{key: 'addList', text: '@CodAddD@', icon: 'AddToShoppingList', uievent: 'addantibioticgroupuievent', onFinish: 'refreshfilter' },
                                              {key: 'deleteList', text: '@CodDelD@', icon: 'RemoveFromShoppingList', uievent: 'deleteantibioticgroupuievent', onFinish: 'refreshfilter' }]
                                },
                                { 'key': 'add', 'text': '@AntManAdd@', 'icon': 'Add', 'uievent': 'addantibioticuievent', 'onFinish': 'refresh',
                                    rules : [{ effect: 'visible', field: 'codingid', rule: '=', value: '21'}]},
                                { 'key': 'edit', 'text': '@AntManEdi@', 'icon': 'Edit', 'uievent': 'editantibioticuievent', 'onSelect': true, 'onFinish': 'update',
                                    rules : [{ effect: 'visible', field: 'codingid', rule: '=', value: '21'}]},                                
                                { 'key': 'delete', 'text': '@AntManDel@', 'icon': 'Delete', 'uievent': 'deleteantibioticuievent', 'onSelect': true, 'onFinish': 'refresh',
                                    rules : [{ effect: 'visible', field: 'codingid', rule: '=', value: '21'}]},   
                                { key: 'addEntry', 'text': '@GenAddE@', 'icon': 'Add', uievent: 'addantibioticentryuievent', onFinish: 'refresh', 
                                  rules : [{ effect: 'visible', field: 'codingid', rule: '!=', value: '21'}] },
                                { key: 'deleteEntry', 'text': '@GenDelB@', 'icon': 'Delete', uievent: 'deleteantibioticentryuievent', onFinish: 'refresh', onSelect: true,
                                  rules : [{ effect: 'visible', field: 'codingid', rule: '!=', value: '21'}] },
                    ],
                    filters: [
                        { key: 'coding', placeholder: '@AntAnt@', fieldName: 'codingid', multiSelect: false, width: 160, optionsName: 'AntibioticGroup', dynamic: true, includeFixed: true, dropdownwidth: 850 }
                    ],
                    filterPresets: [
                        { key: 'filter1', name: '@OrgAll@', default: true, fields: [ { key: 'coding', values: [ '21'] } ] }
                    ],
                    'filterSearch': true,
                    'searchFields': [ 'antibioticname', 'code' ]
                }";

            return JsonConvert.DeserializeObject<ListViewConfig>(view);
        }
    }
}
