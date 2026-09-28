using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    internal class SettingsRecordViewConfig
    {
        internal RecordViewConfig GetView()
        {

            var view = @"{
                            'title': '@GenSet@',
                            'name': 'settings',
                            'type': 'recordview',
                            buttons:
                                [
                                ],
                            'regions': [
                                {
                                    'id': 'generalsettings',
                                    'type': 'listview',
                                    'title': '@TesAddB@',
                                    'listViewName': 'generalsettingslistview'
                                },
                                {
                                    'id': 'AccessionNumber',
                                    'type': 'listview',
                                    'title': '@SpeAcc@',
                                    'listViewName': 'accessionnumberlistview'
                                },
                                {
                                    'id': 'patientreference',
                                    'type': 'listview',
                                    'title': '@SpePat@',
                                    'listViewName': 'patientreferencelistview'
                                }
                            ]
                         }";


            var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

            return result;
        }
    }
}





