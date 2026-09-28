using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    internal class ImportProfileViewConfig
    {
        internal RecordViewConfig GetView()
        {

            var view = @"{
                            'title': '@ImpImp@',
                            'name': 'importprofile',
                            'type': 'recordview',
                            'singleItemName': '@ImpImp@',
                            'buttons': [],
                            'regions': [
                                { 'id': 'importprofile',
                                  'type': 'standard',
                                  'queryName': 'importprofileviewquery'
                                },
                                {
                                  'id': 'importprofilefield',
                                  'type': 'listview',
                                  'title': '@ExpProField@',
                                  'listViewName': 'importprofilefield'
                                }

                            ]
                         }";


            var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

            return result;
        }
    }
}
