using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    internal class FormConfigViewConfig
    {
        internal RecordViewConfig GetView()
        {

            var view = @"{
                            'title': '@ConDef@',
                            'name': 'formConfig',
                            'type': 'recordview',
                            buttons:
                                [
                                    { key: 'addpage', text: '@ConAddW@', icon: 'Add', uievent: 'addpageuievent', onFinish: 'refresh'},
                                    { key: 'editpages', text: '@ConPag@', icon: 'ChevronUnfold10', uievent: 'editpagesuievent', onFinish: 'refresh'}
                                ],
                            'regions': [
                                {
                                  'id': 'pagesinform',
                                  'type': 'crafted',
                                  'title': '',
                                  'name': 'pagesgrid',
                                  'queryName': 'pagesinformquery'
                                }
                            ]
                         }";


            var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

            return result;
        }
    }
}

