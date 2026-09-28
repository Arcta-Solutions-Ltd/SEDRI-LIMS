//using arc.app.Common;
//using arc.common;
//using arc.domain.Configuration.EventsConfig;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//namespace arc.app.Configuration.Events
//{
//    internal class DeleteSpecimenTypeDirectTestDefaultEvent : IRun
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;

//        public DeleteSpecimenTypeDirectTestDefaultEvent(IConfigExtractionUtils configExtractionUtils)
//        {
//            _configExtractionUtils = configExtractionUtils;
//        }

//        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
//        {
//            var newDirectTest = JsonConvert.DeserializeObject<IdModel>(dataToSave);

//            if (newDirectTest.Id.Contains("|"))
//            {
//                var viewId = int.Parse(newDirectTest.Id.Split("|")[0]);
//                var specimenId = int.Parse(newDirectTest.Id.Split("|")[1]);

//                var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(viewId);

//                view.DeleteDefaultTest(specimenId);

//                await _configExtractionUtils.SaveViewAsync(view);
//            }

//            return 0;
//        }
//    }
//}
