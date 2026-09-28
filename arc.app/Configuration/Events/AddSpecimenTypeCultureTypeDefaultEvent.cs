//using arc.app.Common;
//using arc.common;
//using arc.common.Models.Config;
//using arc.domain.Configuration.EventsConfig;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//namespace arc.app.Configuration.Events
//{
//    internal class AddSpecimenTypeCultureTypeDefaultEvent : IRun
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;

//        public AddSpecimenTypeCultureTypeDefaultEvent(IConfigExtractionUtils configExtractionUtils)
//        {
//            _configExtractionUtils = configExtractionUtils;
//        }

//        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
//        {
//            var newCultureType = JsonConvert.DeserializeObject<CultureTypeDefaultModel>(dataToSave);

//            var viewId = newCultureType.Id.Contains("|") ? int.Parse(newCultureType.Id.Split("|")[0]) : int.Parse(newCultureType.Id);

//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(viewId);

//            view.UpdateDefaultCultures(newCultureType.SpecimenTypeId, newCultureType.CultureTypeId);

//            await _configExtractionUtils.SaveViewAsync(view);

//            return 0;
//        }
//    }
//}
