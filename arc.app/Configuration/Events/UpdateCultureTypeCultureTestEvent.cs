//using arc.app.Common;
//using arc.common;
//using arc.common.Models.Config;
//using arc.domain.Configuration.EventsConfig;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//namespace arc.app.Configuration.Events
//{
//    internal class UpdateCultureTypeCultureTestEvent : IRun
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;

//        public UpdateCultureTypeCultureTestEvent(IConfigExtractionUtils configExtractionUtils)
//        {
//            _configExtractionUtils = configExtractionUtils;
//        }

//        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
//        {
//            var newCultureTest = JsonConvert.DeserializeObject<CultureTypeCultureTestModel>(dataToSave);

//            var viewId = newCultureTest.Id.Contains("|") ? int.Parse(newCultureTest.Id.Split("|")[0]) : int.Parse(newCultureTest.Id);

//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(viewId);

//            view.UpdateDefaultCultureTests(newCultureTest.CultureTypeId, newCultureTest.CultureTestId);

//            await _configExtractionUtils.SaveViewAsync(view);

//            return 0;
//        }
//    }
//}
