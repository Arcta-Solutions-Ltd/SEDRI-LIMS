using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class LanguageListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'language',
                            'type': 'ManageList',
                            'title': '@LanMan@',
                            'headerText': '@LanCre@.',
                            'queryName': 'LanguageList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@LanNew@', 'fieldName': 'Value', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column2', 'name': '@LanOld@', 'fieldName': 'Source', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                            ],
                            'buttons': [
                                { key: 'translations', text: '@LanTra@', icon: 'Translate',
                                    buttons: [
                                                { 'key': 'addlanguage', 'text': '@LanAddA@', 'icon': 'Add', uievent: 'addlanguageuievent', onFinish: 'refreshfilter' },
                                                 { 'key': 'deletelanguage', 'text': '@LanDelB@', 'icon': 'Delete', uievent: 'deletelanguageuievent', onFinish: 'refreshfilter' }
                                            ]
                                },
                                { 'key': 'editWord', 'text': '@LanEdiA@', 'icon': 'Add', 'onSelect': true, uievent: 'editworduievent', onFinish: 'updatefromform', 
                                  rules : [{ effect: 'visible', field: 'translationid', rule: '!=', value: '669'}] }
                            ],
                            filters: [
                                { key: 'TranslationId', placeholder: 'Translation', multiSelect: false, width: 140, optionsName: 'Translation', fieldName: 'translationid', dynamic: true, includeFixed: true }
                            ],
                            filterPresets: [
                                { key: 'filter1', name: '@ConDefD@', fields: [ { key: 'TranslationId', values: [ '669'] } ], default: true }
                            ],
                            filterSearch: true,
                            searchFields: [ 'key', 'Value' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
