using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    internal class IqcTestProfileRecordView
    {
        internal RecordViewConfig GetView()
        {
            var view = @"{
              'name':'iqctestprofile',
              'type':'recordview',
              'title':'@ManQcOrg@',
              'singleItemName':'@ManQcOrgAbrA@',
              'buttons':[],
              'regions':[
                {
                  'id':'qcogranismdetails',
                  'type':'standard',
                  'queryName':'qcorganismforiqctestprofileview'
                },
                {
                  'id':'antimicrobialsdisk',
                  'type':'listview',
                  'title':'@ManQcAntDis@',
                  'listViewName':'qcorganismantimicrobialsdisklist'
                },
                {
                  'id':'antimicrobialsmic',
                  'type':'listview',
                  'title':'@ManQcAntMic@',
                  'listViewName':'qcorganismantimicrobialsmiclist'
                }
              ]
            }";

            return JsonConvert.DeserializeObject<RecordViewConfig>(view);
        }
    }
}
