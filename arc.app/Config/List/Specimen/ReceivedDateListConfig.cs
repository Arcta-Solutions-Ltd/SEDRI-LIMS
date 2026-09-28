using arc.domain.Configuration.ListsConfig;
using Newtonsoft.Json;

namespace arc.app.Config.List.Specimen
{
    internal class ReceivedDateListConfig
    {
        internal ListConfig GetList()
        {
            var form = @"{
                            name: 'receivedDate',
                            options: [ { key: '0', text: '@GenTod@' },
                                { key: '1', text: '@GenYes@' },
                                { key: '2', text: '@GenTod@ - 2' },
                                { key: '3', text: '@GenTod@ - 3' },
                                { key: '4', text: '@GenTod@ - 4'},
                                { key: '5', text: '@GenTod@ - 5'},
                                { key: '>5', text: '@GenOld@' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListConfig>(form);

            return result;
        }
    }
}
