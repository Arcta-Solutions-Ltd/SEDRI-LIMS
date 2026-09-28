using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    internal class IqcTestRecordViewConfig
    {
        internal RecordViewConfig GetView()
        {
            var view =
                @"{
                  'name':'iqctests',
                  'type':'recordview',
                  'title':'@QuaIqcTes@',
                  'workflow':'iqctestworkflow',
                  'buttons':[
                    {
                      'key':'edit',
                      'text':'@QuaEdiQcOrg@',
                      'icon':'edit',
                      'uievent':'editiqctestqcorganismsuievent',
                      'onFinish':'refresh',
                      'workflow':true
                    },
                    {
                      'key':'markiqctestcomplete',
                      'text':'@QuaMarCom@',
                      'icon':'accept',
                      'uievent':'markiqctestcompleteuievent',
                      'onFinish':'refresh',
                      'workflow':true
                    }
                  ],
                  'regions':[
                    {
                      'id':'iqctestdetails',
                      'type':'standard',
                      'queryName':'iqctestforrecordview'
                    },
                    {
                      'id':'iqcresultsgrid',
                      'type':'crafted',
                      'name':'iqcresultsgrid',
                      'queryName':'iqcresultsgridquery'
                    }
                  ]
                }";

            return JsonConvert.DeserializeObject<RecordViewConfig>(view);
        }
    }
}
