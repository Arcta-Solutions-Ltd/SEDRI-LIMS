//using arc.app.Common;
//using arc.common;
//using arc.common.Models.Config;
//using arc.domain.Configuration.EventsConfig;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//namespace arc.app.Configuration.Events
//{
//    internal class DeleteSpecimenTypeCultureTypeDefaultEvent : IRun
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;

//        public DeleteSpecimenTypeCultureTypeDefaultEvent(IConfigExtractionUtils configExtractionUtils)
//        {
//            _configExtractionUtils = configExtractionUtils;
//        }

//        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
//        {
//            var newCultureType = JsonConvert.DeserializeObject<CultureTypeDefaultModel>(dataToSave);

//            if (newCultureType.Id.Contains("|"))
//            {
//                var viewId = int.Parse(newCultureType.Id.Split("|")[0]);
//                var specimenId = int.Parse(newCultureType.Id.Split("|")[1]);

//                var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(viewId);

//                view.DeleteDefaultCulture(specimenId);

//                await _configExtractionUtils.SaveViewAsync(view);
//            }

//            return 0;
//        }
//    }
//}
