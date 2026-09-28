//using arc.app.Common;
//using arc.common;
//using arc.domain.Configuration.EventsConfig;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//namespace arc.app.Configuration.Events
//{
//    internal class DeleteCultureTypeCultureTestDefaultEvent : IRun
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;

//        public DeleteCultureTypeCultureTestDefaultEvent(IConfigExtractionUtils configExtractionUtils)
//        {
//            _configExtractionUtils = configExtractionUtils;
//        }

//        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
//        {
//            var newCultureTest = JsonConvert.DeserializeObject<IdModel>(dataToSave);

//            if (newCultureTest.Id.Contains("|"))
//            {
//                var viewId = int.Parse(newCultureTest.Id.Split("|")[0]);
//                var cultureTypeId = int.Parse(newCultureTest.Id.Split("|")[1]);

//                var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(viewId);

//                view.DeleteDefaultCultureTest(cultureTypeId);

//                await _configExtractionUtils.SaveViewAsync(view);
//            }

//            return 0;
//        }
//    }
//}
